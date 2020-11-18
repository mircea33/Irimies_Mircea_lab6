﻿using AutoLotModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Irimies_Mircea_Lab6
{
    
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;

        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        AutoLotEntitiesModel itx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            inventoryViewSource =
            ((System.Windows.Data.CollectionViewSource)this.FindResource("inventoryViewSource"));
            inventoryViewSource.Source = itx.Inventories.Local;
            customerOrdersViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            customerOrdersViewSource.Source = ctx.Orders.Local;
            ctx.Customers.Load();
            itx.Inventories.Load();
            ctx.Orders.Load();
           /// cmbCustomers.ItemsSource = ctx.Customers.Local;
           // cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = itx.Inventories.Local;
           // cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";
            
            BindDataGrid();
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            custIdTextBox.IsEnabled = true;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;

        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {


            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnPrev.IsEnabled = true;
            btnNext.IsEnabled = true;

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            SetValidationBinding();
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;

            }
            else
            if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;

            }

            else
            if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();

                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                SetValidationBinding();
            }
        }
        private void btnNew_ClickInventory(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            carIdTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            btnNewInventory.IsEnabled = false;
            btnEditInventory.IsEnabled = false;
            btnDeleteInventory.IsEnabled = false;
            btnSaveInventory.IsEnabled = true;
            btnCancelInventory.IsEnabled = true;
            btnPrevInventory.IsEnabled = false;
            btnNextInventory.IsEnabled = false;

        }
        private void btnNext_ClickInventory(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevious_ClickInventory(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }
        private void btnCancel_ClickInventory(object sender, RoutedEventArgs e)
        {


            btnNewInventory.IsEnabled = true;
            btnEditInventory.IsEnabled = true;
            btnEditInventory.IsEnabled = true;
            btnSaveInventory.IsEnabled = false;
            btnCancelInventory.IsEnabled = false;
            btnPrevInventory.IsEnabled = true;
            btnNextInventory.IsEnabled = true;

        }
        private void btnDelete_ClickInventory(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            btnNewInventory.IsEnabled = false;
            btnEditInventory.IsEnabled = false;
            btnDeleteInventory.IsEnabled = false;
            btnSaveInventory.IsEnabled = true;
            btnCancelInventory.IsEnabled = true;
            btnPrevInventory.IsEnabled = false;
            btnNextInventory.IsEnabled = false;

        }
        private void btnEdit_ClickInventory(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            btnNewInventory.IsEnabled = false;
            btnEditInventory.IsEnabled = false;
            btnDeleteInventory.IsEnabled = false;
            btnSaveInventory.IsEnabled = true;
            btnCancelInventory.IsEnabled = true;
            btnPrevInventory.IsEnabled = false;
            btnNextInventory.IsEnabled = false;


        }
        private void btnSave_ClickInventory(object sender, RoutedEventArgs e)
        {

            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {
                    inventory = new Inventory()
                    {
                        Color = colorTextBox.Text.Trim(),
                        Make = makeTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    itx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    //salvam modificarile
                    itx.SaveChanges();
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                btnNewInventory.IsEnabled = true;
                btnEditInventory.IsEnabled = true;
                btnSaveInventory.IsEnabled = false;
                btnCancelInventory.IsEnabled = false;

                btnPrevInventory.IsEnabled = true;
                btnNextInventory.IsEnabled = true;
            }
            else
            if (action == ActionState.Edit)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Color = colorTextBox.Text.Trim();
                    inventory.Make = makeTextBox.Text.Trim();
                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                inventoryViewSource.View.MoveCurrentTo(inventory);
                btnNewInventory.IsEnabled = true;
                btnEditInventory.IsEnabled = true;
                btnDeleteInventory.IsEnabled = true;
                btnSaveInventory.IsEnabled = false;
                btnCancelInventory.IsEnabled = false;
                btnPrevInventory.IsEnabled = true;
                btnNextInventory.IsEnabled = true;

            }

            else
            if (action == ActionState.Delete)
            {
                try
                {
                    inventory = (Inventory)customerDataGrid.SelectedItem;
                    itx.Inventories.Remove(inventory);
                    itx.SaveChanges();

                }
                catch (DataException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                btnNewInventory.IsEnabled = true;
                btnEditInventory.IsEnabled = true;
                btnDeleteInventory.IsEnabled = true;
                btnSaveInventory.IsEnabled = false;
                btnCancelInventory.IsEnabled = false;
                btnPrevInventory.IsEnabled = true;
                btnNextInventory.IsEnabled = true;

            }
        }
        private void btnNew0_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            BindingOperations.ClearBinding(carIdTextBox1, TextBox.TextProperty);
            BindingOperations.ClearBinding(custIdTextBox1, TextBox.TextProperty);
            carIdTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            btnNewInventory.IsEnabled = false;
            btnEditInventory.IsEnabled = false;
            btnDeleteInventory.IsEnabled = false;
            btnSaveInventory.IsEnabled = true;
            btnCancelInventory.IsEnabled = true;
            btnPrevInventory.IsEnabled = false;
            btnNextInventory.IsEnabled = false;

        }
        private void btnNext0_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevious0_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToPrevious();
        }
        private void btnCancel0_Click(object sender, RoutedEventArgs e)
        {


            btnNew0.IsEnabled = true;
            btnEdit0.IsEnabled = true;
            btnEdit0.IsEnabled = true;
            btnSave0.IsEnabled = false;
            btnCancel0.IsEnabled = false;
            btnPrev0.IsEnabled = true;
            btnNext0.IsEnabled = true;

        }
        private void btnDelete0_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            btnNew0.IsEnabled = false;
            btnEdit0.IsEnabled = false;
            btnDelete0.IsEnabled = false;
            btnSave0.IsEnabled = true;
            btnCancel0.IsEnabled = true;
            btnPrev0.IsEnabled = false;
            btnNext0.IsEnabled = false;

        }
        private void btnEdit0_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            btnNew0.IsEnabled = false;
            btnEdit0.IsEnabled = false;
            btnDelete0.IsEnabled = false;
            btnSave0.IsEnabled = true;
            btnCancel0.IsEnabled = true;
            btnPrev0.IsEnabled = false;
            btnNext0.IsEnabled = false;
        }
        private void btnSave0_Click(object sender, RoutedEventArgs e)
        {
            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    //instantiem Order entity
                    order = new Order()
                    {

                        CustId = customer.CustId,
                        CarId = inventory.CarId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew0.IsEnabled = true;
                btnEdit0.IsEnabled = true;
                btnSave0.IsEnabled = false;
                btnCancel0.IsEnabled = false;

                btnPrev0.IsEnabled = true;
                btnNext0.IsEnabled = true;

            }
            else
          if (action == ActionState.Edit)
            {
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                    editedOrder.CarId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(selectedOrder);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;
                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Orders.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Order Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals
                             cust.CustId
                             join inv in ctx.Inventories on ord.CarId
                 equals inv.CarId
                             select new
                             {
                                 ord.OrderId,
                                 ord.CarId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.Make,
                                 inv.Color
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }
        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty,
           firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string min length validator
           /// lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValid());
            lastNameTextBox.SetBinding(TextBox.TextProperty,
           lastNameValidationBinding); //setare binding nou
        }
    }
}
