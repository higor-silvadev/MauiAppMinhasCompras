using MauiAppMinhasCompras.Models;
using System;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioPage : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public RelatorioPage()
    {
        InitializeComponent(); // obrigatoriamente chama o XAML
        lstProdutos.ItemsSource = lista;
        CarregarTodosProdutos();
    }

    private async void CarregarTodosProdutos()
    {
        try
        {
            lista.Clear();
            var tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
            AtualizarTotal();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            DateTime inicio = dateInicio.Date;
            DateTime fim = dateFim.Date;

            lista.Clear();

            var produtos = await App.Db.GetAll();
            var filtrados = produtos
                .Where(p => p.DataCadastro >= inicio && p.DataCadastro <= fim)
                .OrderByDescending(p => p.DataCadastro)
                .ToList();

            filtrados.ForEach(p => lista.Add(p));

            AtualizarTotal();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void AtualizarTotal()
    {
        double total = lista.Sum(p => p.Total);
        lblTotal.Text = $"Total do período: {total:C}";
    }
}