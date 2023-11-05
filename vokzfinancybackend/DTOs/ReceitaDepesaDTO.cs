using VokzFinancy.Models;

namespace VokzFinancy.DTOs {

    public class ReceitaDespesaDTO {

        public Conta Conta {get; set;}
        public double ValorDespesa { get; set; }
        public double ValorReceita { get; set; }

    }

}