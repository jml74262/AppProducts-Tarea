using AppProducts.Models;
using AppProducts.Services;
using AppProducts.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppProducts.ViewModels
{
    public class ProductsListViewModel : BaseViewModel
    {
        //variables locales

        //comandos
        private Command _NewCommand;
        public Command NewCommand => _NewCommand ?? (_NewCommand = new Command(NewAction));

        private Command _SelectedCommand;
        public Command SelectedCommand => _SelectedCommand ?? (_SelectedCommand = new Command(SelectedAction));



        //propiedades
        private List<ProductModel> _ListProducts;
        public List<ProductModel> ListProducts
        {
            get => _ListProducts;
            set => SetProperty(ref _ListProducts, value);
        }

        private ProductModel _SelectedProduct;
        public ProductModel SelectedProduct
        {
            get => _SelectedProduct;
            set => SetProperty(ref _SelectedProduct, value);
        }

        //Constructores
        public ProductsListViewModel()
        {
            LoadProducts();
        }

        //Metodos
        private async void LoadProducts()
        {
            IsBusy = true;
            ListProducts = null;
            ApiResponse response = await new ApiService().GetDataAsync("Product");
            if (response == null || !response.IsSuccess)
            {
                // No hubo una respuesta exitosa
                //IsBusy = false;
                //await Application.Current.MainPage.DisplayAlert("AppProducts", $"Error al cargar los productos: {response.Message}", "Ok");
                //return;
            }
            ListProducts = JsonConvert.DeserializeObject<List<ProductModel>>(response.Result.ToString());
            IsBusy = false;
        }
        public void RefreshProducts()
        {
            LoadProducts();
        }

        private void NewAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ProductsDetailView(this));
        }
        private void SelectedAction(object obj)
        {
            if (SelectedProduct == null) {
                return;
            }
            else
            {
                Application.Current.MainPage.Navigation.PushAsync(new ProductsDetailView(this));
            }
            
        }
    }
}
