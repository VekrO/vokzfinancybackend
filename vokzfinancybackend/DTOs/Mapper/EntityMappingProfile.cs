using AutoMapper;
using VokzFinancy.Models;
using VokzFinancy.DTOs;

namespace VokzFinancy.DTOs.Mapper {

    public class EntityMappingProfile : Profile {

        public EntityMappingProfile() {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Despesa, DespesaDTO>().ReverseMap();
            CreateMap<Receita, ReceitaDTO>().ReverseMap();
            CreateMap<Conta, ContaDTO>().ReverseMap();
        }

    }

}