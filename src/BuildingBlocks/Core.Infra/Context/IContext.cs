﻿using Core.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Core.Infra.Context
{
    public interface IContext : IUnitOfWork
    {
        DbSet<T> GetDbSet<T>() where T : class;

        DbConnection GetDbConnection();
    }
}