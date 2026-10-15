using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PetsController : TMSController
    {
        private readonly SuppliersContext _context;

        public PetsController(SuppliersContext context) : base(context)
        {
            _context = context;
        }

        // Ver las mascotas del cliente cuando el cliente las consulta, si no es un cliente el que consulta, las muestra todas.
        // GET: api/Pets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {

            var query = _context.Pets.AsQueryable();

            if (this.IsClient == true) {
              query = query.Include(pet => pet.Specie).ThenInclude(Specie => Specie.Rate ).Where(pet => pet.ClientId == CurrentClient.Id);
        
            }

            return await query.Select(pet => new Pet  (){
                Id = pet.Id,
                Name = pet.Name,
                Age = pet.Age,
                Gender = pet.Gender,
                Birthday = pet.Birthday,
                Colour = pet.Colour,
                Fur = pet.Fur,
                Observations = pet.Observations,
                Photo = pet.Photo,
                Document = pet.Document,
                Weight = pet.Weight,
                Neutered = pet.Neutered,
                RateId  = pet.RateId,
                SpecieId  = pet.SpecieId,
                ClientId = pet.ClientId,
                Height  = pet.Height,
                Specie  = new Specie {
                    Id = pet.Specie.Id,                   
                    Name = pet.Specie.Name,                   
                    Rate = pet.Specie.Rate,                   
                },  
            }).ToListAsync();

        }
        // Ver las mascotas del cliente cuando el cliente las consulta, si no es un cliente el que consulta, las muestra todas.
        // GET: api/Pets
        [HttpGet("get-pets-clients")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPetsClients()
        {

            var query = _context.Pets.AsQueryable();
            query = query.Include(pet => pet.Specie).ThenInclude(Specie => Specie.Rate ).Include(pet => pet.Client).ThenInclude(Client => Client.User);

            return await query.Select(pet => new Pet  (){
                Name = pet.Name,
                Gender = pet.Gender,
                Colour = pet.Colour,
                Observations = pet.Observations,
                Document = pet.Document,
                RateId  = pet.RateId,
                SpecieId  = pet.SpecieId,
                Client  = new Client {
                    Document = pet.Client.Document,
                    User = new User {
                        Email = pet.Client.User.Email,
                        Name = pet.Client.User.Name
                    }
                },
                Specie  = new Specie {
                    Id = pet.Specie.Id,                   
                    Name = pet.Specie.Name,                   
                    Rate = pet.Specie.Rate,                   
                },  
            }).ToListAsync();
        }

        // Obtiene mascotas de cada cliente individualmente
        // GET: api/ForIdAll
        [HttpGet("get-pet-id/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPetId(int id)
        {
            var query = _context.Pets.AsQueryable();
            query = query.Where(pet => pet.ClientId == id);

            return await query.Select(pet => new Pet  (){
                Id = pet.Id,
                Name = pet.Name,
                Age = pet.Age,
                Gender = pet.Gender,
                Birthday = pet.Birthday,
                Colour = pet.Colour,
                Fur = pet.Fur,
                Observations = pet.Observations,
                Photo = pet.Photo,
                Document = pet.Document,
                Weight = pet.Weight,
                Neutered = pet.Neutered,
                RateId  = pet.RateId,
                SpecieId  = pet.SpecieId,
                ClientId = pet.ClientId,
                Height  = pet.Height, 
            }).ToListAsync();
        }

        // GET: api/Pets/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                return NotFound();
            }
            return pet;
        }

        // PUT: api/Pets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPet(int id, Pet pet)
        {
            if (id != pet.Id)
            {
                return BadRequest();
            }

            _context.Entry(pet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost] 
        [Authorize]
        public async Task<ActionResult<Pet>> PostPet(Pet pet)
                {
                    pet.ClientId = this.CurrentClient.Id;
                    _context.Pets.Add(pet);
                    await _context.SaveChangesAsync();

                    return new Pet  (){
                        Id = pet.Id,
                        Name = pet.Name,
                        Age = pet.Age,
                        Gender = pet.Gender,
                        Birthday = pet.Birthday,
                        Colour = pet.Colour,
                        Fur = pet.Fur,
                        Observations = pet.Observations,
                        Photo = pet.Photo,
                        Document = pet.Document,
                        Weight = pet.Weight,
                        Neutered = pet.Neutered,
                        RateId  = pet.RateId,
                        SpecieId  = pet.SpecieId,
                        ClientId = pet.ClientId,
                        Height  = pet.Height, 
                    };
                }

        // DELETE: api/Pets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Pet>> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return pet;
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.Id == id);
        }
    }
}