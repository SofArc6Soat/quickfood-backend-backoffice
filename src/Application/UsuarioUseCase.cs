using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Dtos.Response;

namespace UseCases
{
    public class UsuarioUseCase : BaseUseCase, IUsuarioUseCase
    {
        private readonly ICognitoGateway _cognitoGateway;
        private readonly Func<object, bool> _validacao;

        public UsuarioUseCase(ICognitoGateway cognitoGateway, INotificador notificador, Func<object, bool> validacao)
            : base(notificador)
        {
            _cognitoGateway = cognitoGateway;
            _validacao = validacao;
        }

        public async Task<TokenUsuario?> IdentificarFuncionarioAsync(string email, string senha, CancellationToken cancellationToken) =>
            await _cognitoGateway.IdentifiqueSeAsync(email, null, senha, cancellationToken);

        public async Task<TokenUsuario?> IdentificarClienteCpfAsync(string cpf, string senha, CancellationToken cancellationToken) =>
            await _cognitoGateway.IdentifiqueSeAsync(null, cpf, senha, cancellationToken);

        public async Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(emailVerificacao);

            if (_validacao(emailVerificacao) && await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar a verificação do e-mail: {emailVerificacao.Email}");
            return false;
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(recuperacaoSenha);

            if (_validacao(recuperacaoSenha) && await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro solicitar a recuperacao de senha do e-mail: {recuperacaoSenha.Email}");
            return false;
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(resetSenha);

            if (_validacao(resetSenha) && await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar o reset de senha do e-mail: {resetSenha.Email}");
            return false;
        }
    }
}
