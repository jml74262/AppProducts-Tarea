using AppProducts.Models;
using AppProducts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppProducts.ViewModels
{
    public class ProductsDetailViewModel : BaseViewModel
    {
        //Variables locales

        public readonly ProductsListViewModel ListViewModel;

        //Comandos
        private Command _SaveCommand;
        public Command SaveCommand => _SaveCommand ?? (_SaveCommand = new Command(SaveAction));

        private Command _DeleteCommand;
        public Command DeleteCommand => _DeleteCommand ?? (_DeleteCommand = new Command(DeleteAction));

        
        //Propiedades
        private int _ProductID;
        public int ProductID
        {
            get => _ProductID;
            set => SetProperty(ref _ProductID, value);
        }
        private string _ProductCategory;
        public string ProductCategory
        {
            get => _ProductCategory;
            set => SetProperty(ref _ProductCategory, value);
        }
        private string _ProductModel;
        public string ProductModel
        {
            get => _ProductModel;
            set => SetProperty(ref _ProductModel, value);
        }
        private string _ProductDesigner;
        public string ProductDesigner
        {
            get => _ProductDesigner;
            set => SetProperty(ref _ProductDesigner, value);
        }
        private string _ProductDescription;
        public string ProductDescription
        {
            get => _ProductDescription;
            set => SetProperty(ref _ProductDescription, value);
        }
        private float _ProductPrice;
        public float ProductPrice
        {
            get => _ProductPrice;
            set => SetProperty(ref _ProductPrice, value);
        }
        private string _ProductPicture;
        public string ProductPicture
        {
            get => _ProductPicture;
            set => SetProperty(ref _ProductPicture, value);
        }


        //Constructores

        public ProductsDetailViewModel()
        {

        }
        public ProductsDetailViewModel(ProductsListViewModel listViewModel)
        {
            ListViewModel = listViewModel;
            if (ListViewModel.SelectedProduct == null)
            {
                this.ProductID = 0;
            }
            else
            {
                
                ProductID = listViewModel.SelectedProduct.ID;
                ProductModel = listViewModel.SelectedProduct.Model;
                ProductCategory = listViewModel.SelectedProduct.Category;
                ProductDesigner = listViewModel.SelectedProduct.Designer;
                ProductDescription = listViewModel.SelectedProduct.Descpription;
                ProductPicture = listViewModel.SelectedProduct.Picture;
                ProductPrice = listViewModel.SelectedProduct.Price;
                

            }
        }
        //Metodos
        private async void SaveAction()
        {
            ApiResponse response;
            try
            {
                IsBusy = true;
                ProductModel model = new ProductModel
                {
                    ID = ProductID,
                    Model = ProductModel,
                    Category = ProductCategory,
                    Price = ProductPrice,
                    Descpription = ProductDescription,
                    Designer = ProductDesigner,
                    Picture = ProductPicture
                };
                if (model.ID == 0)
                {
                    //crear nuevo producto
                    response = await new ApiService().PostDataAsync("Product", model);
                }
                else
                {
                    //actualizar producto existente
                    
                    response = await new ApiService().PutDataAsync("Product", model);
                    ListViewModel.SelectedProduct = null;
                }
                //Si no fue satisfactorio
                if (response == null || !response.IsSuccess)
                {
                    //IsBusy = false;
                    //await Application.Current.MainPage.DisplayAlert("AppProducts", response.Message, "ok");
                    //return;
                }
                //Actualizamos
                ListViewModel.RefreshProducts();

                //Cerramos la vista actual
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppProducts", $"Se generó un error al guardar el producto: {ex.Message}", "ok");
            }
        }
        private async void DeleteAction(object obj)
        {
            ApiResponse response;
            try
            {
                if(ProductID != 0)
                {
                    response = await new ApiService().DeleteDataAsync("Product", ProductID);
                    ListViewModel.SelectedProduct = null;
                }

                //Actualizamos
                ListViewModel.RefreshProducts();

                //Cerramos la vista actual
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppProducts", $"Se generó un error al borrar el producto: {ex.Message}", "ok");
            }
        }
    }
}
