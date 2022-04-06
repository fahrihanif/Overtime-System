using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        //Get Data
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var get = repository.Get().Count();
                return get == 0
                    ? NotFound(new { message = "Data Empty" })
                    : (ActionResult)Ok(repository.Get());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //Get Data with parameter primary key
        [HttpGet("{id}")]
        public ActionResult GetById(Key id)
        {
            try
            {
                var get = repository.Get(id);
                return get == null
                    ? NotFound(new { message = "Data Not Found" })
                    : (ActionResult)Ok(get);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        //Insert data to entity
        [HttpPost]
        public virtual ActionResult Post(Entity entity)
        {
            try
            {
                var post = repository.Insert(entity);
                return post == 0
                    ? NotFound(new { message = "Data Failed to Change Please Check Again" })
                    : (ActionResult)Ok(new { message = "Data Saved Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //Update data from existing row in entity
        [HttpPut]
        public virtual ActionResult Update(Entity entity)
        {
            try
            {
                var update = repository.Update(entity);
                return update == 0
                    ? NotFound((new { message = "Data Failed to Change Please Check Again" }))
                    : (ActionResult)Ok(new { message = "Data Has Been Successfully Changed" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //Delete data from row in entity by primary key
        [HttpDelete("{id}")]
        public ActionResult Delete(Key id)
        {
            try
            {
                var delete = repository.Delete(id);
                return delete == 0
                    ? NotFound(new { message = $"{id} Not Found" })
                    : (ActionResult)Ok(new { message = $"Your selected id has been deleted" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
