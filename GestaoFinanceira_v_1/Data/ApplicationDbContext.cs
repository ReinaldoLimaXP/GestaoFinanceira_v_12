using GestaoFinanceira_v_1.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestaoFinanceira_v_1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cliente>()
               .HasOne(c => c.Cidade) // Cada Cliente tem uma Cidade
               .WithMany(ci => ci.Clientes) // Cada Cidade pode ter vários Clientes
               .HasForeignKey(c => c.fk_id_cidades) // A chave estrangeira em Cliente
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Cliente>()
                .HasMany(c => c.Veiculos)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.Fk_id_cliente)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Cliente>()
                .HasMany(c => c.ComprasdoCliente)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.Fk_id_cliente)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Veiculo>()
                .HasMany(c => c.Vendas)
                .WithOne(v => v.Veiculo)
                .HasForeignKey(v => v.Fk_id_veiculo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Venda>()
                .HasOne(v => v.Vistoriador)
                .WithMany(f => f.VendasComoVistoriador)
                .HasForeignKey(v => v.Fk_id_vistoriador)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Venda>()
                .HasOne(v => v.FuncionarioVendedor)
                .WithMany(f => f.VendasComoVendedor)
                .HasForeignKey(v => v.Fk_id_funcionario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Venda>()
                .HasOne(v => v.Caixa_venda)
                .WithMany(f => f.VendasdoCaixa)
                .HasForeignKey(v => v.Fk_id_caixa)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Venda>()
                .HasOne(v => v.Empresa)
                .WithMany(f => f.VendadaEmpresa)
                .HasForeignKey(v => v.Fk_id_empresa)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Fornecedor>()
                .HasMany(c => c.DespesasPagasForncedor)
                .WithOne(v => v.FornecedorDespesa)
                .HasForeignKey(v => v.Fk_id_fornecedor)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlanoDeContas>()
                .HasMany(c => c.DespesasDoPlano)
                .WithOne(v => v.PlanoPag)
                .HasForeignKey(v => v.Fk_id_plano)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Caixa>()
               .HasMany(c => c.SangriaDestinos) //um caixa tem várias sangrias
               .WithOne(v => v.CaixaDestino) // cada sangria tem um caixa
               .HasForeignKey(v => v.Fk_id_caixaDestino)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Caixa>()
                .HasMany(c => c.SangriaOrigem) //um caixa tem várias sangrias
                .WithOne(v => v.CaixaOrigem) // cada sangria tem um caixa origem
                .HasForeignKey(v => v.Fk_id_caixaOrigem)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Endereco>()
                .HasOne(v => v.Cidade)
                .WithMany(f => f.ClientesdeEnderecos)
                .HasForeignKey(v => v.Fk_id_cidades)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Funcionario>()
                .HasOne(v => v.Empresa_fun)
                .WithMany(f => f.FuncionariosdaEmpresa)
                .HasForeignKey(v => v.Fk_id_empresa)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Funcionario>()
                .HasOne(v => v.Funcao_fun)
                .WithMany(f => f.FuncoesdosFuncionarios)
                .HasForeignKey(v => v.Fk_id_funcao)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Despesa>()
                .HasOne(v => v.EmpresaDespesa)
                .WithMany(f => f.DespesadaEmpresa)
                .HasForeignKey(v => v.Fk_id_empresa)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Despesa>()
                .HasOne(v => v.CaixaPagamento)
                .WithMany(f => f.DespesasdoCaixa)
                .HasForeignKey(v => v.Fk_id_caixa)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Recebimento>()
                .HasOne(v => v.EmpresaRecebimento)
                .WithMany(f => f.RecebimentosdaEmpresa)
                .HasForeignKey(v => v.Fk_id_empresa)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Caixa>()
                .HasOne(c => c.BancoCaixa)
                .WithMany(f => f.CaixasdoBanco)
                .HasForeignKey(c => c.Fk_id_banco)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Caixa>()
                .HasOne(v => v.EmpresaCaixa)
                .WithMany(f => f.CaixasdaEmpresa)
                .HasForeignKey(v => v.Fk_id_empresa)
                .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Recebimento>()
			    .HasOne(v => v.CaixaRecebimento)
			    .WithMany(f => f.RecebimentosdoCaixa)
			    .HasForeignKey(v => v.Fk_id_caixa)
			    .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Recebimento>()
			    .HasOne(v => v.DispositivoRec)
			    .WithMany(f => f.RecebimentosdoDispositivo)
			    .HasForeignKey(v => v.Fk_id_dispositivo_rec)
			    .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Recebimento>()
			    .HasOne(v => v.Venda)
			    .WithMany(f => f.RecebimentoLista)
			    .HasForeignKey(v => v.Fk_id_venda)
			    .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Recebimento>()
			    .HasOne(v => v.Plano)
			    .WithMany(f => f.RecebimentosDoplano)
			    .HasForeignKey(v => v.Fk_id_plano)
			    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Caixa>()
                .HasOne(v => v.Funcionario_Caixa)
                .WithMany(f => f.CaixaDoFuncionario)
                .HasForeignKey(v => v.Fk_id_funcionario)
                .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<LogCaixa>()
				.HasOne(v => v.CaixaLog)
				.WithMany(f => f.LogsDoCaixa)
				.HasForeignKey(v => v.Fk_id_caixa)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<LogCaixa>()
				.HasOne(v => v.FuncionarioLog)
				.WithMany(f => f.LogsFuncCaixas)
				.HasForeignKey(v => v.Fk_id_funcionario)
				.OnDelete(DeleteBehavior.Restrict);
		}

        public DbSet<Servico> servicos { get; set; }
        public DbSet<Estado> estados { get; set; }
        public DbSet<Endereco> enderecos { get; set; }
        public DbSet<Cidade> cidades { get; set; }
        public DbSet<Empresa> empresas { get; set; }

        public DbSet<Cliente> clientes { get; set; }

        public DbSet<ClientePF> clientePF { get; set; } = default!;

        public DbSet<ClientePJ> clientePJ { get; set; } = default!;

        public DbSet<Veiculo> veiculos { get; set; }
        public DbSet<MarcaVeiculos> marcaVeiculos { get; set; }
        public DbSet<Cores> cores { get; set; }
        public DbSet<CategoriaVeiculos> categoriaVeiculos { get; set; }

        public DbSet<Funcao> funcao { get; set; }

        public DbSet<Funcionario> funcionarios { get; set; }

        public DbSet<Caixa> caixas { get; set; }

        public DbSet<Venda> Vendas { get; set; }

        public DbSet<Recebimento> Recebimentos { get; set; }

        public DbSet<ItemVenda> ItensVenda { get; set; }

        public DbSet<Dispositivo> Dispositivos { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set; }

        public DbSet<Despesa> Despesas { get; set; }

        public DbSet<PlanoDeContas> PlanoContas { get; set; }

        public DbSet<Encargo> Encargos { get; set; }

        public DbSet<Sangria> Sangrias { get; set; }

        public DbSet<Banco> Bancos { get; set; }

        public DbSet<LogCaixa> LogCaixas { get; set; }

	}
}
