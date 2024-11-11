using GestaoFinanceira_v_1.Components.Pages.Financeiro.Caixas;
using GestaoFinanceira_v_1.Components.Pages.Vendas;
using GestaoFinanceira_v_1.Entidades;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using GestaoFinanceira_v_1.Controllers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;
using FastReport;


namespace GestaoFinanceira_v_1.Utilitarios
{
    public class ImpressaoCaixa
    {
        public static string? nomeUsuario;
        public async Task GerarRelatorioCaixa(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal, bool movimentoPrazo)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {
                movimentoPrazo = false;
                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.StatusVenda.ToUpper() != "ABERTO").ToList());
                    RelatorioCaixa.Add(caixaRelatorio);

                }

                foreach (var item in RelatorioCaixa)
                {
                    item.TotalMovimento = MovimentoCaixa.Sum(m => m.Valor);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Caixa\RelCaixaCompleto.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelCaixas_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioCaixa

        public async Task GerarRelatorioCaixaResumido(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal, bool movimentoPrazo)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {
                movimentoPrazo = false;
                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.StatusVenda.ToUpper() != "ABERTO").ToList());
                    RelatorioCaixa.Add(caixaRelatorio);

                }

                foreach (var item in RelatorioCaixa)
                {
                    item.TotalMovimento = MovimentoCaixa.Sum(m => m.Valor);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Caixa\RelCaixaResumido.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelCaixas_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioCaixaResumido


        public async Task GerarRelatorioRecebimentosCompleto(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {
                
                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.Classificacao=="ENTRADA" && m.Origem!= "Sangria Destino").ToList());
                    RelatorioCaixa.Add(caixaRelatorio);

                }


                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Recebimentos\RelRecebimentoCompleto.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                foreach (var item in RelatorioCaixa)
                {
                    item.TotalMovimento = MovimentoCaixa.Sum(m => m.Valor);
                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelRecebimentos_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioRecebimentos

        public async Task GerarRelatorioRecebimentosResumido(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {

                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.Classificacao == "ENTRADA" && m.Origem != "Sangria Destino").ToList());
                    RelatorioCaixa.Add(caixaRelatorio);

                }

                foreach (var item in RelatorioCaixa)
                {
                    item.TotalMovimento = MovimentoCaixa.Sum(m => m.Valor);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Recebimentos\RelRecebimentoResumido.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelRecebimentosRes_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioRecebimentosResumido

        //############################################################################################
        public async Task GerarRelatorioPagamentosCompleto(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {

                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.Classificacao == "SAÍDA" && m.Origem != "Sangria Origem").ToList());
                    RelatorioCaixa.Add(caixaRelatorio);

                }

                foreach (var item in RelatorioCaixa)
                {
                    item.TotalMovimento = MovimentoCaixa.Sum(m => m.Valor);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Pagamentos\RelPagamentoCompleto.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelRecebimentos_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioPagamentos

        public async Task GerarRelatorioPagamentosResumido(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {

                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.Classificacao == "SAÍDA" && m.Origem != "Sangria Origem").ToList());
                    RelatorioCaixa.Add(caixaRelatorio);

                }

                foreach (var item in RelatorioCaixa)
                {
                    item.TotalMovimento = MovimentoCaixa.Sum(m => m.Valor);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Pagamentos\RelPagamentoResumido.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelRecebimentos_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioPagamento Resumido

        public async Task GerarRelatorioDespesasAPagar(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {

                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.Classificacao == "SAÍDA" && m.Origem != "Sangria Origem").ToList());
                    RelatorioCaixa.Add(caixaRelatorio);
                }

                MovimentoCaixa = MovimentoCaixa.Where(mc=>mc.DataEfetivado == null).ToList();

                foreach (var item in RelatorioCaixa)
                {
                    item.TotalMovimento = MovimentoCaixa.Sum(m => m.Valor); 
                }

                
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Pagamentos\RelDespesasAPagar.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                report1.Prepare();
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);

                var fileName = $"RelRecebimentos_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioPagamento Resumido

        //##########################################################################################

        public async Task GerarRelatorioFinanceiro(List<Caixa> caixas, NavigationManager nav, IJSRuntime jsRuntime, DateOnly dataInicio, DateOnly dataFinal, string? filtro)
        {
            List<Movimento> MovimentoCaixa = new List<Movimento>();
            List<CaixaRelatorio> RelatorioCaixa = new List<CaixaRelatorio>();
            try
            {
               
                foreach (var item in caixas)
                {
                    CaixaRelatorio caixaRelatorio = new CaixaRelatorio(item);
                    caixaRelatorio.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                    caixaRelatorio.EmissorDoRelatorio = nomeUsuario;
                    foreach (var mov in caixaRelatorio.Movimentos)
                    {
                        mov.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                        mov.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                        mov.NomeDoEmissor = nomeUsuario;
                    }
                    MovimentoCaixa.AddRange(caixaRelatorio.Movimentos.Where(m => m.Origem != "Sangria Origem" && m.Origem != "Sangria Destino").ToList()); 
                    RelatorioCaixa.Add(caixaRelatorio);
                }

               
            
                var mesAno = MovimentoCaixa?.FirstOrDefault()?.MesAno;
                decimal? totalEntradas = 0;
                decimal? totalSaidas = 0;
                int cont = 1; 
                var MovimentoGiro = MovimentoCaixa.ToList();
                if ((filtro != null))
                { 
                    Console.WriteLine("Entrou");
                    MovimentoGiro = MovimentoCaixa.Where(m => m.PlanoDeConta==filtro).ToList();
                    if (filtro.Contains("RECEITAS"))
                    {
                        MovimentoGiro = MovimentoCaixa.Where(m=>m.Classificacao=="ENTRADA").ToList();
                        Console.WriteLine("passou aqui!1");
                    }
                    if (filtro.Contains("DESPESAS"))
                    {
                        MovimentoGiro = MovimentoCaixa.Where(m => m.Classificacao == "SAÍDA").ToList();
                        Console.WriteLine("passou aqui21");
                    }
                    
                }
                MovimentoCaixa = MovimentoGiro.ToList();    
                foreach (var item in MovimentoGiro)
                {
                    if(item.MesAno == mesAno)
                    {
                        if (item.Classificacao == "ENTRADA") totalEntradas = item.Valor + totalEntradas;
                        if (item.Classificacao == "SAÍDA") totalSaidas = item.Valor + totalSaidas;
                        if (cont == MovimentoGiro.Count) 
                        {
                            Movimento FechamentoEntradas = new Movimento();
                            FechamentoEntradas.Id = 1;
                            FechamentoEntradas.NumeroOrigem = 1000000;
                            FechamentoEntradas.Origem = "1. RECEITAS";
                            FechamentoEntradas.Tipo = "Entrada";
                            FechamentoEntradas.Classificacao = "ENTRADA";
                            FechamentoEntradas.ClienteConta = "-";
                            FechamentoEntradas.Forma = "-";
                            FechamentoEntradas.Parcela = "-";
                            FechamentoEntradas.Valor = totalEntradas;
                            FechamentoEntradas.CaixaRelatorioId = item.CaixaRelatorioId;
                            FechamentoEntradas.StatusVenda = "Fechado";
                            FechamentoEntradas.PlanoDeConta = "1. RECEITAS";
                            FechamentoEntradas.MesAno = mesAno;
                            FechamentoEntradas.ValorRelatorio = totalEntradas ; ;

                            Movimento FechamentoSaidas = new Movimento();
                            FechamentoSaidas.Id = 5;
                            FechamentoSaidas.NumeroOrigem = 1000000;
                            FechamentoSaidas.Origem = "2. DESPESAS";
                            FechamentoSaidas.Tipo = "Saída";
                            FechamentoSaidas.Classificacao = "SAÍDA";
                            FechamentoSaidas.ClienteConta = "-";
                            FechamentoSaidas.Forma = "-";
                            FechamentoSaidas.Parcela = "-";
                            FechamentoSaidas.Valor = totalSaidas;
                            FechamentoSaidas.CaixaRelatorioId = item.CaixaRelatorioId;
                            FechamentoSaidas.StatusVenda = "Fechado";
                            FechamentoSaidas.PlanoDeConta = "2. DESPESAS";
                            FechamentoSaidas.MesAno = mesAno;
                            FechamentoSaidas.ValorRelatorio = totalEntradas - totalSaidas ; ;

                            Movimento FechamentoPeriodo = new Movimento();
                            FechamentoPeriodo.Id = 100000;
                            FechamentoPeriodo.NumeroOrigem = 1000000;
                            FechamentoPeriodo.Origem = "Resultado do Período";
                            FechamentoPeriodo.Tipo = "-";
                            FechamentoPeriodo.Classificacao = "-";
                            FechamentoPeriodo.ClienteConta = "-";
                            FechamentoPeriodo.Forma = "-";
                            FechamentoPeriodo.Parcela = "-";
                            FechamentoPeriodo.Valor = totalEntradas - totalSaidas;
                            FechamentoPeriodo.CaixaRelatorioId = item.CaixaRelatorioId;
                            FechamentoPeriodo.StatusVenda = "Fechado";
                            FechamentoPeriodo.PlanoDeConta = "Resultado do Período";
                            FechamentoPeriodo.MesAno = mesAno;
                            FechamentoPeriodo.ValorRelatorio = totalEntradas - totalSaidas ; ;

                            FechamentoPeriodo.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                            FechamentoPeriodo.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);

                            FechamentoSaidas.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                            FechamentoSaidas.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);

                            FechamentoEntradas.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                            FechamentoEntradas.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);

                            MovimentoCaixa.Add(FechamentoEntradas);
                            MovimentoCaixa.Add(FechamentoSaidas);
                            MovimentoCaixa.Add(FechamentoPeriodo);
                        }
                    }
                    else
                    {
                        Movimento FechamentoEntradas = new Movimento();
                        FechamentoEntradas.Id = 1;
                        FechamentoEntradas.NumeroOrigem = 1000000; 
                        FechamentoEntradas.Origem = "1. RECEITAS";
                        FechamentoEntradas.Tipo = "Entrada";
                        FechamentoEntradas.Classificacao = "ENTRADA";
                        FechamentoEntradas.ClienteConta = "-";
                        FechamentoEntradas.Forma = "-";
                        FechamentoEntradas.Parcela = "-";
                        FechamentoEntradas.Valor = totalEntradas; 
                        FechamentoEntradas.CaixaRelatorioId = item.CaixaRelatorioId;
                        FechamentoEntradas.StatusVenda = "Fechado";
                        FechamentoEntradas.PlanoDeConta = "1. RECEITAS";
                        FechamentoEntradas.MesAno = mesAno;
                        FechamentoEntradas.ValorRelatorio = totalEntradas - 1000; 

                        Movimento FechamentoSaidas = new Movimento();
                        FechamentoSaidas.Id = 5;
                        FechamentoSaidas.NumeroOrigem = 1000000;
                        FechamentoSaidas.Origem = "2. DESPESAS";
                        FechamentoSaidas.Tipo = "Saída";
                        FechamentoSaidas.Classificacao = "SAÍDA";
                        FechamentoSaidas.ClienteConta = "-";
                        FechamentoSaidas.Forma = "-";
                        FechamentoSaidas.Parcela = "-";
                        FechamentoSaidas.Valor = totalSaidas;
                        FechamentoSaidas.CaixaRelatorioId = item.CaixaRelatorioId;
                        FechamentoSaidas.StatusVenda = "Fechado";
                        FechamentoSaidas.PlanoDeConta = "2. DESPESAS";
                        FechamentoSaidas.MesAno = mesAno;
                        FechamentoSaidas.ValorRelatorio = totalSaidas - 1000; ;

                        Movimento FechamentoPeriodo = new Movimento();
                        FechamentoPeriodo.Id = 100000;
                        FechamentoPeriodo.NumeroOrigem = 1000000;
                        FechamentoPeriodo.Origem = "Resultado do Período";
                        FechamentoPeriodo.Tipo = "-";
                        FechamentoPeriodo.Classificacao = "-";
                        FechamentoPeriodo.ClienteConta = "-";
                        FechamentoPeriodo.Forma = "-";
                        FechamentoPeriodo.Parcela = "-";
                        FechamentoPeriodo.Valor = totalEntradas - totalSaidas;
                        FechamentoPeriodo.CaixaRelatorioId = item.CaixaRelatorioId;
                        FechamentoPeriodo.StatusVenda = "Fechado";
                        FechamentoPeriodo.PlanoDeConta = "Resultado do Período";
                        FechamentoPeriodo.MesAno = mesAno;
                        FechamentoPeriodo.ValorRelatorio = totalEntradas - totalSaidas - 1000; ;

                        FechamentoPeriodo.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                        FechamentoPeriodo.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);

                        FechamentoSaidas.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);
                        FechamentoSaidas.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);

                        FechamentoEntradas.DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue);
                        FechamentoEntradas.DataFim = dataFinal.ToDateTime(TimeOnly.MinValue);

                        //MovimentoCaixa.Add(FechamentoEntradas);
                        //MovimentoCaixa.Add(FechamentoSaidas);
                        MovimentoCaixa.Add(FechamentoEntradas);
                        MovimentoCaixa.Add(FechamentoSaidas);
                        MovimentoCaixa.Add(FechamentoPeriodo); 

                        mesAno = item.MesAno;
                        totalEntradas = 0; 
                        totalSaidas = 0;
                        if (item.Classificacao == "ENTRADA") totalEntradas = item.Valor + totalEntradas;
                        if (item.Classificacao == "SAÍDA") totalSaidas = item.Valor + totalSaidas;

                    }

                    cont++;
                }

                if(MovimentoCaixa.Where(a=>a.Classificacao=="ENTRADA").Sum(a=>a.Valor)==0)
                     MovimentoCaixa = MovimentoCaixa.Where(a => a.Classificacao != "ENTRADA").ToList();

                if (MovimentoCaixa.Where(a => a.Classificacao == "SAÍDA").Sum(a => a.Valor) == 0)
                     MovimentoCaixa = MovimentoCaixa.Where(a => a.Classificacao != "SAÍDA").ToList();




                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Relatorios\Financeiro\RelFinanceiro.frx");
                if (!File.Exists(filePath))
                {
                    var report = new FastReport.Report();
                    //report.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                    report.Dictionary.RegisterBusinessObject(MovimentoCaixa, "MovimentoCaixa", 10, true);
                    report.Report.Save(filePath);

                }

                var report1 = new FastReport.Report();
                report1.Report.Load(filePath);
                //report1.Dictionary.RegisterBusinessObject(RelatorioCaixa, "RelatorioCaixa", 10, true);
                report1.AutoFillDataSet = true;
                report1.Dictionary.RegisterBusinessObject(MovimentoCaixa.ToList(), "MovimentoCaixa", 10, true);
                
                
                report1.Prepare();
                


                
                
                using var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                using var reportStream = new MemoryStream();
                pdfExport.Export(report1, reportStream);
                
               var matrix1 = report1.FindObject("Matrix1") as FastReport.Matrix.MatrixObject;
                Console.WriteLine("valor: " + matrix1.RowCount); 

                var fileName = $"RelCaixas_{Guid.NewGuid()}.pdf";
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
                Console.WriteLine("Erro aqui!: " + ex.Message.ToString());
                throw new Exception(ex.Message);

            }

        }//fim do método GerarRelatórioFiannceiro


    }

   

    public class CaixaRelatorio
    {
        public long Id { get; set; }
        public DateTime? DataAbertura { get; set; }
        public DateTime? Fechamento { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public TimeSpan? HorarioAbertura { get; set; }
        public TimeSpan? HorarioFechamento { get; set; }
        public string? ResponsavelVenda { get; set; }
        public string? TipoOperacional { get; set; }
        public decimal? DinheiroEntrada { get; set; }
        public decimal? CartaoDebitoEntrada { get; set; }
        public decimal? PixEntrada { get; set; }
        public decimal? BaixaCartaoCreditoEntrada { get; set; }
        public decimal? BaixaBoletoEntrada { get; set; }
        public decimal? TransferenciaEntrada { get; set; }
        public decimal? PixSaida { get; set; }
        public decimal? DinheiroSaida { get; set; }
        public decimal? EncargosBancarios { get; set; }
        public decimal? Sangrias { get; set; }
        public decimal? SaldoInicial { get; set; }
        public decimal? TotalEntradas { get; set; }
        public decimal? TotalSaidas { get; set; }
        public decimal? SaldoFinal { get; set; }
        public string? EmissorDoRelatorio { get; set; }
        public string? NumeroCaixa { get; set; }
        public string? MesAno {  get; set; }    
        public List<Movimento>? Movimentos { get; set; }
        public decimal? TotalMovimento { get; set; }    

        public CaixaRelatorio(Caixa caixa)
        {

            Movimentos = new List<Movimento>();
            //MovimentoZero();
            DataAbertura = caixa.Data_abertura.Value.ToDateTime(TimeOnly.MinValue);
            MesAno = GerarMesAno();
            MovimentoEntradaRecebimentos(caixa);
            MovimentoSangriaSaida(caixa);
            MovimentoSaidaDespesas(caixa);
            MovimentoSangriaEntrada(caixa);
            MovimentoSaidaEncargos(caixa);
            Id = caixa.Id_caixa;
            Fechamento = caixa.Data_fechamento?.ToDateTime(TimeOnly.MinValue);//Fechamento = caixa.Data_fechamento.Value.ToDateTime(TimeOnly.MinValue);
            HorarioAbertura = caixa.Horaabertura;
            HorarioFechamento = caixa.Horafechamento; 
            ResponsavelVenda = caixa.Funcionario_Caixa.Nome_completo;
            TipoOperacional = caixa.Tipo;
            if (caixa.Tipo == "Banco" && caixa.BancoCaixa!=null) TipoOperacional = TipoOperacional +" "+caixa.BancoCaixa.Descricao;
            DinheiroEntrada = Movimentos.Where(m => m.Classificacao == "ENTRADA" && m.Tipo == "Dinheiro" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            CartaoDebitoEntrada = Movimentos.Where(m => m.Classificacao == "ENTRADA" && m.Tipo == "Cartão Débito" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            PixEntrada = Movimentos.Where(m => m.Classificacao == "ENTRADA" && m.Tipo == "Pix" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            BaixaCartaoCreditoEntrada = Movimentos.Where(m => m.Classificacao == "ENTRADA" && m.Tipo == "Cartão Crédito" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            BaixaBoletoEntrada = Movimentos.Where(m => m.Classificacao == "ENTRADA" && m.Tipo == "Boleto" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            TransferenciaEntrada = Movimentos.Where(m => m.Classificacao == "ENTRADA" && m.Origem == "Sangria Origem" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            PixSaida = Movimentos.Where(m => m.Classificacao == "SAÍDA" && m.Tipo == "Pix" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            DinheiroSaida = Movimentos.Where(m => m.Classificacao == "SAÍDA" && m.Tipo == "Dinheiro" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            EncargosBancarios = Movimentos.Where(m => m.Classificacao == "SAÍDA" && m.Tipo == "Encargo Bancário" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            Sangrias = Movimentos.Where(m => m.Classificacao == "SAÍDA" && m.Origem == "Sangria Origem" && m.StatusVenda.ToUpper() == "FECHADO").Sum(m => m.Valor);
            SaldoInicial = Convert.ToDecimal(caixa.Saldo_inicial);
            TotalEntradas = Convert.ToDecimal(caixa.Total_entradas);
            TotalSaidas = Convert.ToDecimal(caixa.Total_saidas);
            SaldoFinal = Convert.ToDecimal(caixa.Saldo_final);
            NumeroCaixa = caixa.Id_caixa + "/" + DataAbertura.Value.Year; 
        }


        //métodos de entrada de valor
        public void MovimentoEntradaRecebimentos(Caixa caixa)
        {
            if (caixa.RecebimentosdoCaixa != null)
            {
                foreach (var item in caixa.RecebimentosdoCaixa.ToList())
                {
                    Movimento movimento = new Movimento();
                    movimento.Id = item.Id_recebimento;
                    movimento.NumeroOrigem = item.Id_recebimento;
                    if (item.Fk_id_venda == null) movimento.Origem = item.Descricao;
                    if (item.Fk_id_venda != null) movimento.Origem = "Venda";
                    movimento.Tipo = item.TipoPagamento;
                    if (item.Fk_id_venda != null) movimento.ClienteConta = item.Venda.Cliente is ClientePF clientePF ? clientePF.Nome_cli : (item.Venda.Cliente as ClientePJ)?.RazaoSocial_PJ;
                    if (item.Fk_id_venda == null) movimento.ClienteConta = "-";
                    movimento.Classificacao = "ENTRADA";
                    if (item.Fk_id_venda == null) movimento.Forma = "-";
                    if (item.Fk_id_venda != null) movimento.Forma = item.Venda.FormaPagamento;
                    if (item.Fk_id_venda == null) movimento.Parcela = "-";
                    if (item.Fk_id_venda != null) movimento.Parcela = item.Nparcela + "/" + item.Venda.TotalParcelas;
                    movimento.Valor = Convert.ToDecimal(item.ValorParcela);
                    movimento.CaixaRelatorioId = caixa.Id_caixa;
                    movimento.StatusVenda = item.StatusRecebimento;
                    movimento.MesAno = MesAno;
                    movimento.PlanoDeConta = item.Plano?.Codigo + " " + item.Plano?.Descricao;
                    movimento.ValorRelatorio = Convert.ToDecimal(item.ValorParcela);
                    movimento.DataInicio = DataInicio;
                    movimento.DataFim = DataFim;
                    if(item.Fk_id_dispositivo_rec!=null) movimento.Dispositivo = item.DispositivoRec.Descricao;
                    if(item.Fk_id_dispositivo_rec != null) movimento.CustoDispositivo = Convert.ToDecimal(item.AliquotaParcela);
                    if(item.DataVencimento!=null) movimento.Vencimento = item.DataVencimento.ToDateTime(TimeOnly.MinValue);
                    movimento.DataEfetivado = item.DataPagamento.ToDateTime(TimeOnly.MinValue);
                    movimento.HorarioEfetivado = item.HoraPagamento; 
                    Movimentos.Add(movimento);
                }
            }

        }

        public void MovimentoSangriaEntrada(Caixa caixa)
        {
            if (caixa.SangriaDestinos != null)
            {
                foreach (var item in caixa.SangriaDestinos.ToList())
                {
                    Movimento movimento = new Movimento();
                    movimento.Id = item.Id_sangria;
                    movimento.NumeroOrigem = item.Id_sangria;
                    movimento.Origem = "Sangria Destino";
                    movimento.Tipo = "Entrada";
                    movimento.Classificacao = "ENTRADA";
                    movimento.ClienteConta = "-";
                    movimento.Forma = "-";
                    movimento.Parcela = "-";
                    movimento.Valor = Convert.ToDecimal(item.ValorSangria);
                    movimento.CaixaRelatorioId = caixa.Id_caixa;
                    movimento.StatusVenda = "Fechado";
                    movimento.MesAno = MesAno;
                    movimento.ValorRelatorio = Convert.ToDecimal(item.ValorSangria);//para validação
                    movimento.DataInicio = DataInicio;
                    movimento.DataFim = DataFim;
                    Movimentos.Add(movimento);
                }
            }
        }


        //métodos de saída de valor 
        public void MovimentoSaidaDespesas(Caixa caixa)
        {
            if (caixa.DespesasdoCaixa != null)
            {
                foreach (var item in caixa.DespesasdoCaixa.ToList())
                {
                    Movimento movimento = new Movimento();
                    movimento.Id = item.Id_despesa;
                    movimento.NumeroOrigem = item.Id_despesa;
                    movimento.Origem = item.Descricao;
                    movimento.Tipo = item.TipoPagamento;
                    movimento.Classificacao = "SAÍDA";
                    movimento.Forma = "-";
                    movimento.Parcela = "-";
                    movimento.Valor = Convert.ToDecimal(item.Valor);
                    movimento.CaixaRelatorioId = caixa.Id_caixa;
                    movimento.StatusVenda = item.Status_des;
                    movimento.MesAno = MesAno;
                    movimento.PlanoDeConta = item.PlanoPag?.Codigo+" "+item.PlanoPag?.Descricao;
                    movimento.ValorRelatorio = Convert.ToDecimal(item.Valor);
                    movimento.DataInicio = DataInicio;
                    movimento.DataFim = DataFim;
                    movimento.DataEfetivado = item.DataPagamento?.ToDateTime(TimeOnly.MinValue);
                    movimento.HorarioEfetivado = item.HoraPagamento;
                    if (item.Vencimento != null) movimento.Vencimento = item.Vencimento?.ToDateTime(TimeOnly.MinValue);
                    movimento.FornecedorDespesa = item.FornecedorDespesa?.Nome_fantasia; 
                    Movimentos.Add(movimento);

                }
            }
        }

        public void MovimentoSangriaSaida(Caixa caixa)
        {
            if (caixa.SangriaOrigem != null)
            {
                foreach (var item in caixa.SangriaOrigem.ToList())
                {
                    Movimento movimento = new Movimento();
                    movimento.Id = item.Id_sangria;
                    movimento.NumeroOrigem = item.Id_sangria;
                    movimento.Origem = "Sangria Origem";
                    movimento.Tipo = "Saída";
                    movimento.ClienteConta = "-";
                    movimento.Classificacao = "SAÍDA";
                    movimento.Forma = "-";
                    movimento.Parcela = "-";
                    movimento.Valor = Convert.ToDecimal(item.ValorSangria);
                    movimento.CaixaRelatorioId = caixa.Id_caixa;
                    movimento.StatusVenda = "Fechado";
                    movimento.MesAno = MesAno;
                    movimento.ValorRelatorio = Convert.ToDecimal(item.ValorSangria);
                    movimento.DataInicio = DataInicio;
                    movimento.DataFim = DataFim;
                    Movimentos.Add(movimento);

                }
            }

        }

        public void MovimentoSaidaEncargos(Caixa caixa)
        {
            if (caixa.RecebimentosdoCaixa != null)
            {
                foreach (var item in caixa.RecebimentosdoCaixa.ToList())
                {
                    if (item.AliquotaParcela != null)
                    {
                        var valorAliq = Convert.ToDecimal(item.AliquotaParcela.Trim().ToString());
                        if (valorAliq > 0)
                        {
                            Movimento movimento = new Movimento();
                            movimento.Id = item.Id_recebimento;
                            movimento.NumeroOrigem = item.Id_recebimento;
                            movimento.Origem = "Encargos";
                            movimento.Tipo = "Encargo Bancário";
                            movimento.ClienteConta = item.DispositivoRec.Descricao;
                            movimento.Classificacao = "SAÍDA";
                            if (item.Fk_id_venda == null) movimento.Forma = "-";
                            if (item.Fk_id_venda != null) movimento.Forma = item.Venda.FormaPagamento;
                            movimento.Parcela = "-";
                            movimento.Valor = Convert.ToDecimal(valorAliq);
                            movimento.CaixaRelatorioId = caixa.Id_caixa;
                            movimento.StatusVenda = item.StatusRecebimento;
                            movimento.MesAno = MesAno;
                            movimento.PlanoDeConta = "2.9 Encargo Bancário";
                            movimento.ValorRelatorio = Convert.ToDecimal(valorAliq);
                            movimento.DataInicio = DataInicio;
                            movimento.DataFim = DataFim;
                            movimento.Dispositivo = item.DispositivoRec.Descricao;
                            movimento.CustoDispositivo = Convert.ToDecimal(valorAliq);
                            movimento.DataEfetivado = item.DataPagamento.ToDateTime(TimeOnly.MinValue);
                            movimento.HorarioEfetivado = item.HoraPagamento;
                            if (item.DataVencimento != null) movimento.Vencimento = item.DataVencimento.ToDateTime(TimeOnly.MinValue);
                            movimento.FornecedorDespesa = item.DispositivoRec.Descricao;
                            Movimentos.Add(movimento);
                        }
                    }
                }
            }
        }

        public string GerarMesAno()
        {
            string resultado = "";
            int mes = DataAbertura.Value.Month;
            int ano = DataAbertura.Value.Year;

            switch (mes)
            {
                case 1:
                    resultado = "JANEIRO/" + ano;
                    break;
                case 2:
                    resultado = "FEVEREIRO/" + ano;
                    break;
                case 3:
                    resultado = "MARÇO/" + ano;
                    break;
                case 4:
                    resultado = "ABRIL/" + ano;
                    break;
                case 5:
                    resultado = "MAIO/" + ano;
                    break;
                case 6:
                    resultado = "JUNHO/" + ano;
                    break;
                case 7:
                    resultado = "JULHO/" + ano;
                    break;
                case 8:
                    resultado = "AGOSTO/" + ano;
                    break;
                case 9:
                    resultado = "SETEMBRO/" + ano;
                    break;
                case 10:
                    resultado = "OUTUBRO/" + ano;
                    break;
                case 11:
                    resultado = "NOVEMBRO/" + ano;
                    break;
                case 12:
                    resultado = "DEZEMBRO/" + ano;
                    break;
                default:
                    resultado = "Mês inválido";
                    break;
            }
            return resultado;
        }
    }

    public class Movimento
    {
        public long Id { get; set; }
        public string? Origem { get; set; }
        public long? NumeroOrigem { get; set; }
        public string? ClienteConta { get; set; }
        public string? Forma { get; set; }
        public string? Tipo { get; set; }
        public string? Parcela { get; set; }
        public decimal? Valor { get; set; }
        public string? PlanoDeConta { get; set; } //descrição do plano de conta
        public string? tipoOperacao { get; set; } //Para identificar o tipo de operação.   
        public string? Classificacao { get; set; }//Para informar seo  movimentão é "entrada" ou uma "saída" 
        public string? StatusVenda { get; set; } //Para informar se a venda a prazo foi fechado ou não
        public long CaixaRelatorioId { get; set; } //buscar o ID da Classe Caixa 
        public string? MesAno {  get; set; }  //receber o mês e ano  
        public decimal? ValorRelatorio { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? NomeDoEmissor { get; set; }
        public string? Dispositivo { get; set; }//máquina da operação 
        public decimal? CustoDispositivo { get; set; }//custo da operação de receboimento
        public DateTime? Vencimento { get; set; } 
        public DateTime? DataEfetivado { get; set; } //data de pagamento ou recebimento
        public TimeSpan? HorarioEfetivado{ get; set; } //horário de pagamento ou recebimento
        public string? FornecedorDespesa { get; set; }
}
}