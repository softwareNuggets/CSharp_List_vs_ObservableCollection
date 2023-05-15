using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace List_vs_ObservableCollection
{
	// follow along with project from my GitHub Account
	// look for CS_List_vs_ObservableCollection

	public partial class MainWindow : Window
	{
		//List<People> AllCustomers = new List<People>();
		ObservableCollection<People> AllCustomers = new ObservableCollection<People>();

		//list = UI will not be update on any activity
		//ObservableCollection = UI will be updated when items are added or removed

		//List                 - none
		//ObservableCollection - inherits INotifyCollectionChanged
		public MainWindow()
		{
			InitializeComponent();
			LoadAllCustomers();

			//In order for the DataGrid user interface to update automatically
			//when items are added to or removed from the source data, the DataGrid must be
			//bound to a collection that implements the INotifyCollectionChanged interface,
			//such as an ObservableCollection<T>.

			GridCustomer.ItemsSource = AllCustomers;
		}

		private void LoadAllCustomers()
		{
			People v1 = new People();
			v1.ID = "1";
			v1.Name = "Candy Necklace";
			v1.Email = "CNecklace@email.com";
			AllCustomers.Add(v1);
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			if (BtnSave.Content == null) return;

			switch(BtnSave.Content.ToString().ToUpper())
			{
				case "SAVE":
					SaveRecord();
					ClearAllDataEntry();
					break;
				case "UPDATE":
					UpdateRecord(this.tb_ID.Text.ToString());
					ClearAllDataEntry();
					break;
			}
			
		}

		private void SaveRecord()
		{
			People person = new People();
			person.ID = this.tb_ID.Text.ToString();
			person.Name = this.tb_FullName.Text.ToString();
			person.Email = this.tb_Email.Text.ToString();
			AllCustomers.Add(person);
		}

		private void UpdateRecord(string ID)
		{
			//When the LINQ statement is executed, it searches for a customer
			//with a matching ID and returns the reference to that customer. This means
			//that person points to the same object in memory as the customer within
			//the AllCustomers collection.

			//By modifying the properties of person, you are indeed modifying the
			//corresponding customer object within the AllCustomers collection. This is
			//because person and the customer object are references to the same underlying
			//memory location.  Therefore, any changes made to the properties of person
			//will be reflected in the AllCustomers collection.


			var person = AllCustomers.Where(p => p.ID == ID).FirstOrDefault();

			if (person != null)
			{
				person.Name = this.tb_FullName.Text.ToString();
				person.Email = this.tb_Email.Text.ToString();

			}
			
		}

		private void GridCustomer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (GridCustomer.CurrentItem == null) return;

			try
			{

				var person = (People)GridCustomer.CurrentItem;
				if (person != null)
				{
					this.tb_ID.Text = person.ID.ToString();
					this.tb_FullName.Text = person.Name.ToString();
					this.tb_Email.Text = person.Email.ToString();

					this.tb_ID.IsEnabled = false;
					this.BtnSave.Content = "Update";
					this.BtnDelete.Visibility = Visibility.Visible;
				}
			}
			catch
			{
				ClearAllDataEntry();
			}
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			var records = GridCustomer.ItemsSource;
			ClearAllDataEntry();
		}

		private void ClearAllDataEntry()
		{
			this.tb_ID.Text = string.Empty;
			this.tb_FullName.Text = string.Empty;
			this.tb_Email.Text = string.Empty;
			this.BtnSave.Content = "Save";
			this.tb_ID.IsEnabled = true;
			this.BtnDelete.Visibility = Visibility.Collapsed;
		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{
			//When you remove an item from an ObservableCollection, the collection will raise
			//the CollectionChanged event. This event can be used to update the UI or
			//other objects that are bound to the collection.

			foreach(var p in AllCustomers)
			{
				if(p.ID == this.tb_ID.Text.ToString())
				{
					AllCustomers.Remove(p);
					break;
				}
			}

			ClearAllDataEntry();

		}
	}
}
