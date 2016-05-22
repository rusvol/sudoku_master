using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace SudokuMaster
{
    public class ResultProcessor
    {

        string[,] all_horizontal = new string[9, 9];
        List<string> list_numbers = new List<string>();
        List<string> inserted_removed = new List<string>();
        int stop_algorithm = 0;
       
        
       public  string FillTheBoard(string FileName){
           
           FileName = Path.Combine(@"C:\Users\Public",System.IO.Path.GetFileName(FileName));
         //  FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.GetFileName(FileName));
            StreamReader reader = new StreamReader(FileName);
            string inputLine = "";
           
           int s = 0;
            
           string value_char = "";
            string table_values = "";
            table_values = "<br/><br/><div class=\"container\" id=\"dv_Table\" >" +
                 "<table  class = \"table table-bordered\"  style=\"font-weight:700\" >";

            while ((inputLine = reader.ReadLine()) != null)
            {
                table_values = table_values  + "<tr>";
                for (int y = 0; y < 9; y++)
                {
                    value_char = inputLine.Substring(s + y, 1);
                    table_values = table_values  + "<td>"  + value_char  +  "</td>";
                }
                table_values = table_values  + "</tr>";
            }
            table_values = table_values + "</table><br/><br/><button type=\"button\" class=\"btn btn-primary\"  onclick =\"SolveThePuzzle()\">Solve the puzzle</button></div>";
       return table_values;
       } 
        
       public List<string>  ReadingFile(string FileName)
        {
            string Result = "";
           
            FileName = Path.Combine(@"C:\Users\Public",
                                        System.IO.Path.GetFileName(FileName));
            StreamReader reader = new StreamReader(FileName);
            string inputLine = "";
            List<List<string>> data = new List<List<string>>();
            int i = 0;
            int s = 0;
            
            string[,] all_vertical = new string[9, 9];
            string[,] all_result = new string[9, 9];

            List<List<string>> input_req = new List<List<string>>();
            List<string> cross_match = new List<string>();
            
            for (int x = 1; x < 10; x++)
            {
                list_numbers.Add(x.ToString());
            }
            string value_char = "";
            
           
            while ((inputLine = reader.ReadLine()) != null)
            {
                List<string> input_match = new List<string>();
                for (int y = 0; y < 9; y++)
                {
                    value_char = inputLine.Substring(s + y, 1);
                    all_horizontal[i, y] = value_char;
                    if (value_char.ToLower() != "x")
                    {
                        input_match.Add(value_char);
                    }
                }
                i = i + 1;
                input_req.Add(list_numbers.Except(input_match).ToList());

            }


            /*  Start  Preparing  lists  for comparing algorithm  */

            List<List<string>> first_req = new List<List<string>>();
            List<List<string>> second_req = new List<List<string>>();
            List<List<string>> third_req = new List<List<string>>();


            for (int a = 0; a < 9; a++)
            {
                if (a < 3)
                {
                    first_req.Add(CubeValues(0, 3, 0, 3));
                    second_req.Add(CubeValues(0, 3, 3, 6));
                    third_req.Add(CubeValues(0, 3, 6, 9));
                }
                if (a > 2 && a < 6)
                {
                    first_req.Add(CubeValues(3, 6, 0, 3));
                    second_req.Add(CubeValues(3, 6, 3, 6));
                    third_req.Add(CubeValues(3, 6, 6, 9));
                }
                if (a > 5)
                {
                    first_req.Add(CubeValues(6, 9, 0, 3));
                    second_req.Add(CubeValues(6, 9, 3, 6));
                    third_req.Add(CubeValues(6, 9, 6, 9));
                }
            }

            bool ret = false;
            for (int n = 0; n < 9; n++)
            {
                inserted_removed.Clear();
                stop_algorithm = 0;
                ret  =  SettingValues(n, input_req, first_req, second_req, third_req);
                if (stop_algorithm == 0) {
                    Result = "Sorry.This Sudoku puzzle can be solved by program.";
                    break;
                }
            }

            string table_values = "";
            string fileName = FileName.Replace(".txt", ".sln.txt");
            //string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            //"Files", name_part);
         
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
          
            List<string> ret_result = new List<string>();
            if (Result == "")
            {
                ret_result = WriteFile(fileName);
                if (ret_result[0] == "Successfull")
                {
                    Result = "The Sudoku puzzle solved results: <br/>" + ret_result[1] + "<br/><br/>  saved in file:   " + fileName;
                    table_values = ret_result[2];
                }
                else
                {
                    Result = ret_result[0];
                }
            }
           

            return new List<string> { Result, table_values };
        }

        public List<string> WriteFile(string FileName)
        {

            string Result = "";
            string table_values = "";
            string file_line = "";
            string file_values = "";
            try
            {
                using (System.IO.StreamWriter file =
                         new System.IO.StreamWriter(FileName))
                {
                   
                    table_values = "<br/><br/><div class=\"container\" id=\"dv_Table\" >" +
                          "<table  class = \"table table-bordered\" style=\"font-weight:700\" >";

                        for (int r = 0; r < 9; r++)
                    {
                        table_values = table_values + "<tr>";
                        for (int p = 0; p < 9; p++)
                        {
                            table_values = table_values + "<td>" + all_horizontal[r, p] + "</td>";
                            file_line = file_line + all_horizontal[r, p];
                            if (all_horizontal[r, p].ToLower() == "x") {
                                Result = "Sorry.This Sudoku puzzle can be solved by program.";
                            }
                        }
                        file.WriteLine(file_line);
                        file_values = file_values + "<br/>" + file_line;
                        file_line = "";
                        table_values = table_values + "</tr>";
                    }
                        table_values = table_values + "</table></div><br/><br/> ";
                }
                if (Result == "")
                {
                    Result = "Successfull";
                }
            }
            catch (Exception exception)
            {
                Result = exception.ToString();
            }
            finally
            {
                GC.Collect();
            }

            return new List<string> { Result, file_values, table_values };
        }


        public Boolean  SettingValues( int n, List<List<string>> input_req,
                               List<List<string>> first_req, List<List<string>> second_req, List<List<string>> third_req )
        {

            bool ret = false;
            List<List<string>> vertical_list = new List<List<string>>();
            List<List<string>> vertical_list_match = new List<List<string>>();
           
            for (int l = 0; l < 9; l++)
            {
                List<string> vertical_req = new List<string>();
                for (int g = 0; g < 9; g++)
                {
                    vertical_req.Add(all_horizontal[g, l]);
                }
                vertical_list_match.Add(vertical_req);

            }
            for (int k = 0; k < 9; k++)
            {
                vertical_list.Add(list_numbers.Except(vertical_list_match[k]).ToList());
            }


            
            List<string> l_checked = new List<string>();
            string value_char = "";
            

            for (int m = 0; m < 9; m++)
            {
                List<string> compare_horizontal = new List<string>();
                List<string> compare_vertical = new List<string>();

                if (m < 3)
                {
                    compare_horizontal = input_req[n].Intersect(first_req[n]).ToList();
                }
                if (m > 2 && m < 6)
                {
                    compare_horizontal = input_req[n].Intersect(second_req[n]).ToList();
                }
                if (m > 5)
                {
                    compare_horizontal = input_req[n].Intersect(third_req[n]).ToList();
                }
                value_char = all_horizontal[n, m];

                if (value_char.ToLower() == "x")
                {
                    l_checked = compare_horizontal.Except(inserted_removed).ToList();

                    for (int j = 0; j < l_checked.Count; j++)
                    {
                        if (vertical_list[m].IndexOf(l_checked[j]) != -1)
                        {
                            compare_vertical.Add(l_checked[j]);
                        }
                    }
                    if (compare_vertical.Count == 1)
                    {
                        all_horizontal[n, m] = compare_vertical[0];
                        vertical_list[m].Remove(compare_vertical[0]);
                        inserted_removed.Add(compare_vertical[0]);
                        stop_algorithm = stop_algorithm + 1;
                    }
                }

            }


            if (input_req[n].Except(inserted_removed).ToList().Count > 0 && stop_algorithm != 0)
            {
                ret = SettingValues(n, input_req, first_req, second_req, third_req);
                return true;
           }
            else {
                return false;
            }
            
        }


        public List<string> CubeValues( int l_start, int l_end, int p_start, int p_end)
        {
            List<string> first_9 = new List<string>();

            string value_char = "";
            for (int a = l_start; a < l_end; a++)
            {
                for (int b = p_start; b < p_end; b++)
                {
                    value_char = all_horizontal[a, b];
                    if (value_char.ToLower() != "x")
                    {
                        first_9.Add(value_char);
                    }

                }
            }
            return list_numbers.Except(first_9).ToList();
        }

    }
}