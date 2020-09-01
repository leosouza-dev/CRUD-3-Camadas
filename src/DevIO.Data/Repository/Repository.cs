using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly MeuDbContext Db;
        protected readonly DbSet<TEntity> DbSet; // atalho para o DbSet (sem DbSet ficaria assim - ver método adicionar)

        protected Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            // Db.Set<TEntity>().Add(entity);
            DbSet.Add(entity);
            await SavaChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SavaChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            //DbSet.Remove(await DbSet.FindAsync(id)); //opção mais simples

            //opção
            var entity = new TEntity { Id = id }; // evita ir no banco buscar antes de remover - isso da pq tds herdam de TEntity
            DbSet.Remove(entity);
            //DbSet.Remove(new TEntity { Id = id }); // removendo uma linha
            await SavaChanges();
        }

        public async Task<int> SavaChanges()
        {
            return await Db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Db?.Dispose(); // ? - se ele existir faça o dispose (evita uma nullReferenceException)
        }
    }
}
