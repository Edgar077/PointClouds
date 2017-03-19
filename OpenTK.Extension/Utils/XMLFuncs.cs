// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace OpenTKExtension
{
  public static class XMLFuncs
  {
    public static DataTable CreateNewTable(string TableName, string[] Fields)
    {
      DataTable dataTable = new DataTable(TableName);
      DataColumn[] dataColumnArray = new DataColumn[1]
      {
        new DataColumn()
      };
      dataColumnArray[0].DataType = System.Type.GetType("System.Int32");
      dataColumnArray[0].ColumnName = "Count";
      dataColumnArray[0].AutoIncrement = true;
      dataColumnArray[0].ReadOnly = true;
      dataTable.Columns.Add(dataColumnArray[0]);
      dataTable.PrimaryKey = dataColumnArray;
      for (int index = 0; index < Fields.Length; ++index)
      {
        DataColumn column = !Fields[index].StartsWith("dbl") ? (!Fields[index].StartsWith("int") ? new DataColumn(Fields[index], System.Type.GetType("System.String")) : new DataColumn(Fields[index], System.Type.GetType("System.Int32"))) : new DataColumn(Fields[index], System.Type.GetType("System.double"));
        dataTable.Columns.Add(column);
        column.Dispose();
      }
      return dataTable;
    }

    public static void CreateColumn(DataTable t, Control c)
    {
      if (t.Rows.Count == 0)
      {
        DataRow row = t.NewRow();
        t.Rows.Add(row);
      }
      if (c.Name.StartsWith("txt"))
      {
        TextBox textBox = (TextBox) c;
        if (t.Columns.IndexOf(c.Name) < 0)
        {
          t.Columns.Add(new DataColumn(c.Name)
          {
            DataType = !c.Name.EndsWith("Name") ? (!c.Name.StartsWith("txtInt") ? System.Type.GetType("System.double") : System.Type.GetType("System.Int32")) : System.Type.GetType("System.String")
          });
          DataRow dataRow = t.Rows[0];
          if (textBox.Text.Trim() != "")
            dataRow[c.Name] = (object) textBox.Text;
        }
        textBox.DataBindings.Clear();
        textBox.DataBindings.Add("Text", (object) t, c.Name, true, DataSourceUpdateMode.OnPropertyChanged);
      }
      else if (c.Name.StartsWith("radio"))
      {
        if (t.Columns.IndexOf(c.Name) < 0)
          t.Columns.Add(new DataColumn(c.Name)
          {
            DataType = System.Type.GetType("System.Boolean")
          });
        RadioButton radioButton = (RadioButton) c;
        radioButton.DataBindings.Clear();
        radioButton.DataBindings.Add("Checked", (object) t, c.Name, true, DataSourceUpdateMode.OnPropertyChanged);
      }
      else if (c.Name.StartsWith("chk"))
      {
        if (t.Columns.IndexOf(c.Name) < 0)
          t.Columns.Add(new DataColumn(c.Name)
          {
            DataType = System.Type.GetType("System.Boolean")
          });
        CheckBox checkBox = (CheckBox) c;
        checkBox.DataBindings.Clear();
        checkBox.DataBindings.Add("Checked", (object) t, c.Name, true, DataSourceUpdateMode.OnPropertyChanged);
      }
      else
      {
        if (!c.Name.StartsWith("cmb"))
          return;
        if (t.Columns.IndexOf(c.Name) < 0)
          t.Columns.Add(new DataColumn(c.Name)
          {
            DataType = System.Type.GetType("System.String")
          });
        ComboBox comboBox = (ComboBox) c;
        comboBox.DataBindings.Clear();
        comboBox.DataBindings.Add("Text", (object) t, c.Name, true, DataSourceUpdateMode.OnPropertyChanged);
      }
    }

    public static DataTable MakeTableFromDataGrid(DataGridView grid, string TableName, DataSet data)
    {
      DataTable table;
      if (data.Tables.IndexOf(TableName) < 0)
      {
        table = new DataTable(TableName);
        data.Tables.Add(table);
      }
      else
        table = data.Tables[TableName];
      if (table.Columns.IndexOf("Count") < 0)
      {
        DataColumn[] dataColumnArray = new DataColumn[1]
        {
          new DataColumn()
        };
        dataColumnArray[0].DataType = System.Type.GetType("System.Int32");
        dataColumnArray[0].ColumnName = "Count";
        dataColumnArray[0].AutoIncrement = true;
        dataColumnArray[0].ReadOnly = true;
        table.Columns.Add(dataColumnArray[0]);
        table.PrimaryKey = dataColumnArray;
      }
      foreach (DataGridViewColumn dataGridViewColumn in (BaseCollection) grid.Columns)
      {
        DataColumn column = new DataColumn(dataGridViewColumn.Name);
        column.DataType = !column.ColumnName.StartsWith("int") ? (!column.ColumnName.StartsWith("string") ? System.Type.GetType("System.double") : System.Type.GetType("System.String")) : System.Type.GetType("System.Int32");
        if (table.Columns.IndexOf(dataGridViewColumn.Name) < 0)
          table.Columns.Add(column);
        table.Columns[dataGridViewColumn.Name].Caption = dataGridViewColumn.HeaderText;
      }
      if (grid.DataSource == null)
        grid.Columns.Clear();
      grid.DataSource = (object) table;
      grid.Columns["Count"].Visible = false;
      foreach (DataGridViewColumn dataGridViewColumn in (BaseCollection) grid.Columns)
        dataGridViewColumn.HeaderText = table.Columns[dataGridViewColumn.Name].Caption;
      return table;
    }

    public static class FileReader
    {
      public static List<string[]> ReadFile(string FileName)
      {
        List<string[]> list = new List<string[]>();
        try
        {
          StreamReader streamReader = new StreamReader(FileName);
          while (!streamReader.EndOfStream)
          {
            string[] strArray = XMLFuncs.FileReader.Trata(streamReader.ReadLine()).Split();
            list.Add(strArray);
          }
          streamReader.Close();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.ToString());
        }
        return list;
      }

      private static string Trata(string linha)
      {
        string newValue = 1.5.ToString().Substring(1, 1);
        linha = linha.Replace(".", newValue);
        linha = linha.Replace(",", newValue);
        linha = linha.Trim().Replace("     ", " ");
        linha = linha.Replace("    ", " ");
        linha = linha.Replace("   ", " ");
        linha = linha.Replace("  ", " ");
        return linha;
      }
    }
  }
}
