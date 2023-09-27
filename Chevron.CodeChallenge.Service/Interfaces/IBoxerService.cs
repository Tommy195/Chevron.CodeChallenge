using Chevron.CodeChallenge.Interfaces;
using Chevron.CodeChallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Chevron.CodeChallenge.Service.Interfaces
{
    public interface IBoxerService
    {
        Task<List<BoxerDTO>> GetAll();

        Task<BoxerDTO> Get(Guid id);

        void PostSimulatingError(BoxerDTO value);

        Task<BoxerDTO> Post(BoxerDTO value);

        Task<BoxerDTO> Put(Guid id, BoxerDTO value);

        void Delete(Guid id);
    }
}
