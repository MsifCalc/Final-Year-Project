using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MeltCalc.Helpers;
using MeltCalc.Model;

namespace MeltCalc.ViewModel
{
	public class Step14Presenter : BasePresenter
	{
		private static readonly LooseMdb _looseMdb = new LooseMdb();

		private readonly ContentControl _groupBox;
		private readonly List<TextBox> _boxes;
		private readonly ComboBox _comboBox;
		private readonly Materials _material;
		private readonly DataTable _table;

		public Step14Presenter(ContentControl groupBox)
		{
			_groupBox = groupBox;

			_material = _groupBox.Material();
			_table = _looseMdb.Reader.FetchTable(_material.ToTableName());

			_boxes = _groupBox.FindVisualChild<TextBox>().ToList();
			_comboBox = _groupBox.FindVisualChild<ComboBox>().SingleOrDefault();
			
			try
			{
				ValidateData();
				InitializeVariants();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// TODO: Ѕыло бы лучше, если прив€зать значени€ по имени колонки, а не по индексу.
		private void SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FillEditBoxes(_material.ToTableName(), _comboBox.SelectedIndex, _boxes);
		}

		private static void FillEditBoxes(string tableName, int index, IList<TextBox> boxes)
		{
			_looseMdb.FillBoxes(tableName, index, boxes, 1);
		}

		private void InitializeVariants()
		{
			// ReSharper disable PossibleNullReferenceException
			for (var i = 0; i < _table.Rows.Count; i++)
			{
				_comboBox.Items.Add(i);
			}

			_comboBox.SelectionChanged += SelectionChanged;
			_comboBox.SelectedIndex = 0;
			// ReSharper restore PossibleNullReferenceException
		}

		private void ValidateData()
		{
			if (_comboBox == null)
			{
				var message = "Combobox not found for " + _material;
				MessageBox.Show(message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
				throw new ApplicationException(message);
			}

			if (_boxes.Count == 0)
			{
				var message = "Boxes not found for " + _material;
				MessageBox.Show(message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
				throw new ApplicationException(message);
			}

			var adder = 1;
			if (_material == Materials.»звестковоћагнитный‘люс)
			{
				adder = 2;
			}

			if (_table.Columns.Count != _boxes.Count + adder) // 1 - это колонка Index.
			{
				var message = "Columns count should be equal to text boxes count in " + _material;
				MessageBox.Show(message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
				throw new ApplicationException(message);
			}
		}
	}
}