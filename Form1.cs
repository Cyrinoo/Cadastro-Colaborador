using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
// para criptografar a SENHA
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // para colocar no banco de dados
            MySqlConnection conn = new MySqlConnection("Server=127.0.0.1;Uid=root;Pwd=usbw;Database=empresa;Persist Security Info=True;");
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO usuario(nome, senha) VALUES (@nome, @senha);", conn);
                cmd.Parameters.AddWithValue("@nome", nome.Text);
                cmd.Parameters.AddWithValue("@senha", CriarMD5Hash(senha.Text));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // para inserir uma quebra de linha
            richTextBox1.AppendText(nome.Text + Environment.NewLine);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("Server=127.0.0.1;Uid=root;Pwd=usbw;Database=empresa;Persist Security Info=True;");
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT nome FROM usuario WHERE nome LIKE @nome;", conn);
                cmd.Parameters.AddWithValue("@nome", textBox3.Text + '%');
                MySqlDataReader reader = cmd.ExecuteReader();

                StringBuilder nomesEncontrados = new StringBuilder();

                while (reader.Read())
                {
                    nomesEncontrados.AppendLine(reader["nome"].ToString());
                }

                if (nomesEncontrados.Length > 0)
                {
                    richTextBox1.Text = nomesEncontrados.ToString();
                }
                else
                {
                    richTextBox1.Text = "Nenhum resultado encontrado.";
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string CriarMD5Hash(string input)
        {
            // Código para criar o hash MD5 permanece inalterado
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
