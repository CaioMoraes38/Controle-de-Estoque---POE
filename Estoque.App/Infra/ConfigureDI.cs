using AutoMapper;
using Estoque.App.Cadastros;
using Estoque.App.Models;
using Estoque.App.Outros;
using Estoque.Domain.Base;
using Estoque.Domain.Entities;
using Estoque.Repository.Context;
using Estoque.Repository.Repository;
using Estoque.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Estoque.App.Infra
{
    public static class ConfigureDI
    {
        public static ServiceCollection? Services;

        public static ServiceProvider? ServicesProvider;

        public static void ConfiguraServices()
        {
            Services = new ServiceCollection();
            var strCon = File.ReadAllText("Config/DatabaseSettings.txt");
            Services.AddDbContext<MySqlContext>(options =>
            {
                options.LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging();
              //  options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                //options.EnableSensitiveDataLogging();


                options.UseMySql(strCon, ServerVersion.AutoDetect(strCon), opt =>
                {
                    opt.CommandTimeout(180);
                    opt.EnableRetryOnFailure(5);
                });
            });

            // Repositories
            Services.AddScoped<IBaseRepository<Usuario>, BaseRepository<Usuario>>();            
            Services.AddScoped<IBaseRepository<Fornecedor>, BaseRepository<Fornecedor>>();
            Services.AddScoped<IBaseRepository<Categoria>, BaseRepository<Categoria>>();
            Services.AddScoped<IBaseRepository<Produto>, BaseRepository<Produto>>();
            Services.AddScoped<IBaseRepository<Venda>, BaseRepository<Venda>>();

            // Services
            Services.AddScoped<IBaseService<Usuario>, BaseService<Usuario>>();    
            Services.AddScoped<IBaseService<Fornecedor>, BaseService<Fornecedor>>();
            Services.AddScoped<IBaseService<Categoria>, BaseService<Categoria>>();
            Services.AddScoped<IBaseService<Produto>, BaseService<Produto>>();
            Services.AddScoped<IBaseService<Venda>, BaseService<Venda>>();

            // Formulários
            Services.AddTransient<Login, Login>();
            Services.AddTransient<CadastroUsuario, CadastroUsuario>();
            Services.AddTransient<CadastroCategoria, CadastroCategoria>();
            Services.AddTransient<CadastroProduto, CadastroProduto>();          
            Services.AddTransient<CadastroFornecedor, CadastroFornecedor>();
            Services.AddTransient<CadastroVenda, CadastroVenda>();

            // Mapping
            Services.AddSingleton(new MapperConfiguration(config =>
            {
                config.CreateMap<Usuario, UsuarioModel>();
                config.CreateMap<Fornecedor, FornecedorModel>();                 
                config.CreateMap<Categoria, Categoria>();
                config.CreateMap<Produto, ProdutoModel>()
                    .ForMember(d => d.Categoria, d => d.MapFrom(x => x.Categoria!.Nome))
                    .ForMember(d => d.IdCategoria, d => d.MapFrom(x => x.Categoria!.Id));
                config.CreateMap<Venda, VendaModel>()
                    .ForMember(d => d.IdFornecedor, d => d.MapFrom(x => x.Fornecedor!.Id))
                    .ForMember(d => d.Fornecedor, d => d.MapFrom(x => x.Fornecedor!.Nome))
                    .ForMember(d => d.IdUsuario, d => d.MapFrom(x => x.Usuario!.Id))
                    .ForMember(d => d.Usuario, d => d.MapFrom(x => x.Usuario!.Nome));

                config.CreateMap<VendaItem, VendaItemModel>()
                    .ForMember(d => d.IdProduto, d => d.MapFrom(x => x.Produto!.Id))
                    .ForMember(d => d.Produto, d => d.MapFrom(x => x.Produto!.Nome));

            }).CreateMapper());

            ServicesProvider = Services.BuildServiceProvider();
        }
    }
}
