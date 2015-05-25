using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleInstance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            List<Person> list = new List<Person>();
            list.Add(new Person() { Name = "Martin", Surname = "Hromek", Age = 28 });
            list.Add(new Person() { Name = "Marie", Surname = "Vrzalova", Age = 26 });
            list.Add(new Person() { Name = "Eva", Surname = "Kolomaznikova", Age = 56 });
            list.Add(new Person() { Name = "Eva", Surname = "Brezovska", Age = 24 });
            
            

            Console.WriteLine("before sorting:");
            Print(list);
            Console.WriteLine("");
            list = list.OrderBy(l => l.Name).ThenBy(u => u.Surname).ToList();
            Console.WriteLine("after sorting:");
            Print(list);

            Console.ReadLine();
        }

        private void Print(List<Person> list)
        {
            foreach (Person item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

    public class Person
    {
        public string Name;
        public string Surname;
        public int Age;

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.Name, this.Surname, this.Age);
        }
    }
}
