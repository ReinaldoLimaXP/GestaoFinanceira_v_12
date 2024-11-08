using GestaoFinanceira_v_1.Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using GestaoFinanceira_v_1.Components.Pages.RelatoriosPages;


namespace GestaoFinanceira_v_1.Utilitarios
{
    public class ImpressaoVendas
    {
        //Caminho Relatório => indicar o local onde será salva o relatório

        public async Task GerarRelatorioVendaCompleta(List<Venda> Vendas, List<ItemVenda> ItensVenda, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal)
        {
            try
            {
                var RelatorioVendas = ConverterVendaParaVendasCompletas(Vendas);
                var Itens = ConverterItemVendaParaItensVendaCompleta(ItensVenda);
                if (RelatorioVendas != null)
                {
                    RelatorioVendas[0].DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    RelatorioVendas[0].DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                }


                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios/RelatorioVendas/RelVendasCompleto.frx");
                //var filePath = "Relatorios/RelatorioVendas/RelVendasCompleto.frx";
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioVendas, "RelatorioVendas", 10, true);
                    report.Dictionary.RegisterBusinessObject(Itens, "ItensVenda", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioVendas, "RelatorioVendas", 10, true);
                report1.Dictionary.RegisterBusinessObject(Itens, "ItensVenda", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelVendasCompleto_{Guid.NewGuid()}.pdf";
                var filePathTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RelatoriosTemp", fileName);

                // Cria a pasta se não existir
                Directory.CreateDirectory(Path.GetDirectoryName(filePathTemp));

                // Salva o arquivo PDF temporariamente na pasta wwwroot/RelatoriosTemp
                File.WriteAllBytes(filePathTemp, reportStream.ToArray());

                // Gera a URL para o arquivo temporário
                var url = nav.ToAbsoluteUri($"/RelatoriosTemp/{fileName}");

                //// Abre o relatório em uma nova guia
                //nav.NavigateTo(url.ToString(), true);

                await jsRuntime.InvokeVoidAsync("NovaGuia", url.ToString()); //alteração 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }//fim do método GerarVendaCompleta

        public async Task GerarRelatorioVendaResumida(List<Venda> Vendas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal)
        {
            try
            {
                var RelatorioVendasResumido = ConverterVendaParaVendasCompletas(Vendas);
                if (RelatorioVendasResumido != null)
                {
                    RelatorioVendasResumido[0].DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    RelatorioVendasResumido[0].DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\RelatorioVendas\RelVendasResumida.frx");
                //var filePath = Path.Combine(@"Relatorios\RelatorioVendas\RelVendasResumida.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioVendasResumido, "RelatorioVendasResumido", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioVendasResumido, "RelatorioVendasResumido", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelVendasCompleto_{Guid.NewGuid()}.pdf";
                var filePathTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RelatoriosTemp", fileName);

                // Cria a pasta se não existir
                Directory.CreateDirectory(Path.GetDirectoryName(filePathTemp));

                // Salva o arquivo PDF temporariamente na pasta wwwroot/RelatoriosTemp
                File.WriteAllBytes(filePathTemp, reportStream.ToArray());

                // Gera a URL para o arquivo temporário
                var url = nav.ToAbsoluteUri($"/RelatoriosTemp/{fileName}");

                //// Abre o relatório em uma nova guia
                //nav.NavigateTo(url.ToString(), true);

                jsRuntime.InvokeVoidAsync("NovaGuia", url.ToString()); //alteração 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        } //fim do método GerarRelatorioVendaResumida

        public async Task GerarRecibo(List<Venda> Vendas, List<ItemVenda> ItensVenda, NavigationManager nav, IJSRuntime jsRuntime)
        {
            try
            {
                var Recibos = ConverterVendaParaReciboVendas(Vendas);
                var Itens = ConverterItemVendaParaItensRecibo(ItensVenda);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\RelatorioRecibos\RelRecibos.frx");
                //var filePath = Path.Combine(@"Relatorios\RelatorioRecibos\RelRecibos.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(Recibos, "Recibos", 10, true);
                    report.Dictionary.RegisterBusinessObject(Itens, "Itens", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(Recibos, "Recibos", 10, true);
                report1.Dictionary.RegisterBusinessObject(Itens, "Itens", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelVendasCompleto_{Guid.NewGuid()}.pdf";
                var filePathTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RelatoriosTemp", fileName);

                // Cria a pasta se não existir
                Directory.CreateDirectory(Path.GetDirectoryName(filePathTemp));

                // Salva o arquivo PDF temporariamente na pasta wwwroot/RelatoriosTemp
                File.WriteAllBytes(filePathTemp, reportStream.ToArray());

                // Gera a URL para o arquivo temporário
                var url = nav.ToAbsoluteUri($"/RelatoriosTemp/{fileName}");

                //// Abre o relatório em uma nova guia
                //nav.NavigateTo(url.ToString(), true);

                jsRuntime.InvokeVoidAsync("NovaGuia", url.ToString()); //alteração 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        } //fim do método Gerar recibo

        public List<VendaCompleta> ConverterVendaParaVendasCompletas(List<Venda> vendas)
        {
            return vendas.Select(venda => new VendaCompleta(venda)).ToList();
        }

        public List<ItensVendaCompleta> ConverterItemVendaParaItensVendaCompleta(List<ItemVenda> itensVenda)
        {
            return itensVenda.Select(item => new ItensVendaCompleta(item)).ToList();
        }

        public List<ReciboVendas> ConverterVendaParaReciboVendas(List<Venda> vendas)
        {
            return vendas.Select(venda => new ReciboVendas(venda)).ToList();
        }

        public List<ItensRecibo> ConverterItemVendaParaItensRecibo(List<ItemVenda> itens)
        {
            return itens.Select(item => new ItensRecibo(item)).ToList();
        }


    }//fim da classe Impressão 

    public class VendaCompleta
    {
        public long Id { get; set; }
        public DateTime DataVistoria { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public TimeSpan Horario { get; set; }
        public string? Vistoriador { get; set; }
        public string? Cliente { get; set; }
        public string? TipoVeiculo { get; set; }
        public string? Veiculo { get; set; }
        public string? FormaPagamento { get; set; }
        public string? TipoRecebimento { get; set; }
        public string? Parcelamento { get; set; }
        public string? EmissorDoRelatorio { get; set; }
        public decimal? Valor { get; set; }

        public VendaCompleta(Venda venda)
        {
            Id = venda.Id_venda;
            DataVistoria = venda.DataVenda.ToDateTime(new TimeOnly(0, 0)); // Convertendo DateOnly para DateTime
            DataInicio = venda.DataVenda.ToDateTime(new TimeOnly(0, 0)); // Assumindo que a data de início é a mesma da venda
            DataFim = venda.DataVenda.ToDateTime(new TimeOnly(23, 59)); // Assumindo que a data de fim é no mesmo dia
            Horario = TimeSpan.Parse(venda.Hora ?? "00:00"); // Convertendo string para TimeSpan
            Vistoriador = venda.Vistoriador?.Nome_completo;
            Cliente = venda.Cliente is ClientePF clientePF ? clientePF.Nome_cli : (venda.Cliente as ClientePJ)?.RazaoSocial_PJ;
            TipoVeiculo = venda.Veiculo?.Categoria_vei?.NomeCategoria;
            Veiculo = venda.Veiculo?.Modelo_vei;
            FormaPagamento = venda.FormaPagamento;
            TipoRecebimento = venda.TipoPagamento;
            Parcelamento = venda.TotalParcelas;
            EmissorDoRelatorio = venda.FuncionarioVendedor?.Nome_completo;
            Valor = Convert.ToDecimal(venda.ValorFinal);
        }
    } //Fim da classe Venda completa. 

    public class ItensVendaCompleta
    {
        public int Id { get; set; }
        public string? Servico { get; set; }
        public string? Und { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public long? IdVendasCompleta { get; set; }

        public ItensVendaCompleta(ItemVenda itemVenda)
        {
            Id = (int)itemVenda.Id_ItemVenda;
            Servico = itemVenda.Servico?.Nome_ser;
            Und = "un"; // Supondo que a unidade seja sempre "un", mas pode ser ajustado conforme necessário.
            Quantidade = (decimal)itemVenda.Quantidade;
            ValorUnitario = (decimal)itemVenda.ValorUnitario;
            ValorTotal = (decimal)itemVenda.ValorTotalItens;
            IdVendasCompleta = itemVenda.Venda?.Id_venda;
        }
    }//fim da classe items Venda completa

    public class ReciboVendas
    {
        public long Id { get; set; }
        public string? NumeroRecibo { get; set; }
        public DateTime DataVistoria { get; set; }
        public TimeSpan Horario { get; set; }
        public string? Vistoriador { get; set; }
        public string? Atendente { get; set; }
        public string? NomeCliente { get; set; }
        public string? CpfCnpjCliente { get; set; } // CPF ou CNPJ   
        public string? TipoVeiculo { get; set; }
        public string? Veiculo { get; set; }
        public string? Placa { get; set; }
        public string? FormaPagamento { get; set; }
        public string? TipoRecebimento { get; set; }
        public string? Parcelamento { get; set; }
        public string? EncargoBancario { get; set; }
        public string? Observacao { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? Total { get; set; }

        public ReciboVendas(Venda venda)
        {
            Id = venda.Id_venda;
            NumeroRecibo = venda.Id_venda.ToString(); // ou outro campo para número de recibo
            DataVistoria = venda.DataVenda.ToDateTime(TimeOnly.Parse(venda.Hora));
            Horario = TimeSpan.Parse(venda.Hora);
            Vistoriador = venda.Vistoriador?.Nome_completo;
            Atendente = venda.FuncionarioVendedor?.Nome_completo;
            NomeCliente = venda.Cliente is ClientePF clientePF ? clientePF.Nome_cli : (venda.Cliente as ClientePJ)?.RazaoSocial_PJ;
            CpfCnpjCliente = venda.Cliente is ClientePF clientePF1 ? clientePF1.Cpf_cli : (venda.Cliente as ClientePJ)?.CNPJ_PJ;
            TipoVeiculo = venda.Veiculo?.Categoria_vei?.NomeCategoria; // Supondo que `NomeCategoria` seja o nome do tipo de veículo
            Veiculo = venda.Veiculo?.Modelo_vei;
            Placa = venda.Veiculo?.Placa_vei;
            FormaPagamento = venda.FormaPagamento;
            TipoRecebimento = venda.TipoPagamento;
            Parcelamento = venda.TotalParcelas;
            EncargoBancario = venda.PorcentagemAliquota + " %"; // Convertendo porcentagem para string
            Observacao = venda.Observacoes;
            SubTotal = (decimal?)venda.ValorTotal;
            Desconto = (decimal?)venda.Desconto;
            Total = (decimal?)venda.ValorFinal;
        }
    }//fim da classe ReciboVendas

    public class ItensRecibo
    {
        public long Id { get; set; }
        public string? Descricao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public long? IdReciboVendas { get; set; }

        public ItensRecibo(ItemVenda itemVenda)
        {
            Id = itemVenda.Id_ItemVenda;
            Descricao = itemVenda.Servico.Nome_ser;
            Quantidade = (decimal?)itemVenda.Quantidade;
            ValorUnitario = (decimal?)itemVenda.ValorUnitario;
            ValorTotal = (decimal?)itemVenda.ValorTotalItens;
            IdReciboVendas = itemVenda.Venda != null ? (long?)itemVenda.Venda.Id_venda : null;
        }
    }


}// fim namespace