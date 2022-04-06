using API.Contexts;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class GeneralRepository<Context, Entity, Key> : IRepository<Entity, Key>
        where Entity : class
        where Context : MyContext
    {
        private readonly MyContext myContext;
        private readonly DbSet<Entity> entities;

        public GeneralRepository(MyContext myContext)
        {
            this.myContext = myContext;
            entities = myContext.Set<Entity>();
        }

        //This method used to delete row in entity by Primary Key
        public int Delete(Key key)
        {
            var check = entities.Find(key);
            if (check != null)
            {
                entities.Remove(check);
                var result = myContext.SaveChanges();
                return result;
            }
            return 0;
        }

        //This method used to get all row in entity
        public IEnumerable<Entity> Get()
        {
            return entities.ToList();
        }

        //This method used to get a row in entity by Primary Key
        public Entity Get(Key key)
        {
            return entities.Find(key);
        }

        //This method used to insert a row to entity
        public int Insert(Entity entity)
        {
            entities.Add(entity);
            var result = myContext.SaveChanges();
            return result;
        }

        //This method used to update a row in entity
        public virtual int Update(Entity entity)
        {
            myContext.Entry(entity).State = EntityState.Modified;
            var result = myContext.SaveChanges();
            return result;
        }
    }
}
