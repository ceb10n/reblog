using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHibernate.Tool.hbm2ddl;
using reblog.App;
using reblog.App.Domain;
using reblog.App.Repository;
using reblog.App.Service;
using reblog.Models;

namespace reblog_database
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = "updating schema";
            var schema = new SchemaExport(NHibernateHelper.Configuration);
            schema.Drop(true, true);
            schema.Create(true, true);
            //var schema = new SchemaUpdate(NHibernateHelper.Configuration);
            //schema.Execute(true, true);

            var adminservice = new AdminService(new AdminRepository(NHibernateHelper.OpenSession()));

            var user = new RegisterModel
            {
                Name = "Rafael de Oliveira Marques",
                Nick = "cebiooon",
                Email = "rafaelomarques@gmail.com",
                Password = "123456"
            };

            adminservice.Register(user);
            status = "ok!";
        }

        private string status
        {
            set
            {
                button1.Text = value;
                button1.Update();
            }
        }
    }
}
