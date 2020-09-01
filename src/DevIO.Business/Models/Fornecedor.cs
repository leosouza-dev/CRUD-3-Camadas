using System.Collections.Generic;

namespace DevIO.Business.Models
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public bool Ativo { get; set; }
        /* EF Relations */
        public Endereco Endereco { get; set; } // fornecedor tem 1 endereço
        public IEnumerable<Produto> Produtos { get; set; } // fornecedor tem varios produtos

    }
}
