using Chevron.CodeChallenge.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Chevron.CodeChallenge.Models;
using Chevron.CodeChallenge.Interfaces;
using Chevron.CodeChallenge.Service.Interfaces;

namespace Chevron.CodeChallenge.Service.Implementations
{
    public class BoxerService : IBoxerService
    {
        private readonly IBoxerRepository _boxerRepository;
        private readonly IUnitOfWork _uow;

        public BoxerService(IBoxerRepository boxerRepository, IUnitOfWork uow)
        {
            _boxerRepository = boxerRepository;
            _uow = uow;
        }

        public async Task<List<BoxerDTO>> GetAll()
        {
            var boxers = await _boxerRepository.GetAll();
            var boxersDTOs = new List<BoxerDTO>();
            
            foreach (var boxer in boxers) 
            {
                var boxerDTO = new BoxerDTO();
                boxerDTO.Description = boxer.Description;
                boxerDTO.Name = boxer.Name;
                boxerDTO.Nationality = boxer.Nationality;
                boxerDTO.Id = boxer.Id;
                boxersDTOs.Add(boxerDTO);
            }

            return boxersDTOs;
        }

        public async Task<BoxerDTO> Get(Guid id)
        {
            var boxer = await _boxerRepository.GetById(id);
            var boxerDTO = new BoxerDTO();
            
            boxerDTO.Description = boxer.Description;
            boxerDTO.Name = boxer.Name;
            boxerDTO.Nationality = boxer.Nationality;
            boxerDTO.Id = boxer.Id;

            return boxerDTO;
        }

        public void PostSimulatingError(BoxerDTO value)
        {
            var boxer = new Boxer(value.Description, value.Name, value.Nationality);
            _boxerRepository.Add(boxer);
        }

        public async Task<BoxerDTO> Post(BoxerDTO value)
        {
            var boxer = new Boxer(value.Description, value.Name, value.Nationality);

            _boxerRepository.Add(boxer);

            await _uow.Commit();

            var addedBoxer = await _boxerRepository.GetById(boxer.Id);

            var boxerDTO = new BoxerDTO();

            boxerDTO.Description = addedBoxer.Description;
            boxerDTO.Name = addedBoxer.Name;
            boxerDTO.Nationality = addedBoxer.Nationality;
            boxerDTO.Id = addedBoxer.Id;

            return boxerDTO;
        }

        public async Task<BoxerDTO> Put(Guid id, BoxerDTO value)
        {
            var boxer = new Boxer(id, value.Description, value.Name, value.Nationality);

            _boxerRepository.Update(boxer);

            await _uow.Commit();

            var updatedBoxer = await _boxerRepository.GetById(id);

            var boxerDTO = new BoxerDTO();

            boxerDTO.Description = updatedBoxer.Description;
            boxerDTO.Name = updatedBoxer.Name;
            boxerDTO.Nationality = updatedBoxer.Nationality;
            boxerDTO.Id = updatedBoxer.Id;

            return boxerDTO;
        }

        public async void Delete(Guid id)
        {
            _boxerRepository.Remove(id);

            await _uow.Commit();
        }
    }
}
