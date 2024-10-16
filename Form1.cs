using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Зад_4
{
    public partial class Form1 : Form
    {
        DataBase dataBase = new DataBase(); // Объект для работы с базой данных
        SqlDataAdapter sqlDataAdapter; // Адаптер для работы с данными из базы
        SqlCommand sqlCommand; // Команда для выполнения SQL-запросов
        DataTable dataTable; // Таблица данных
        public Form1()
        {
            InitializeComponent();
            InitializeDatabase(); // Инициализация подключения к базе данных
            LoadData(); // Загрузка данных в DataGridView
        }
        // Инициализация базы данных
        private int InitializeDatabase()
        {
            try
            {
                dataBase.openConnection(); // Открытие подключения к базе данных
                // Проверим доступность базы данных с помощью простого SQL-запроса
                sqlCommand = new SqlCommand("SELECT 1", dataBase.getConnection());
                sqlCommand.ExecuteNonQuery();
                return 0; // Успех
            }
            catch (Exception ex)
            {
                // В случае ошибки покажем сообщение об ошибке
                MessageBox.Show("Ошибка инициализации базы данных: " + ex.Message);
                return -1; // Ошибка
            }
            finally
            {
                dataBase.closedConnection(); // Закрытие подключения в любом случае
            }
        }
        // Загрузка данных из базы данных в DataGridView
        private int LoadData()
        {
            try
            {
                dataBase.openConnection(); // Открываем соединение
                dataTable = new DataTable(); // Создаём новую таблицу данных
                // Выбираем все данные из таблицы Finance_db
                sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Finance_db", dataBase.getConnection());
                sqlDataAdapter.Fill(dataTable); // Заполняем таблицу данными
                dataGridView1.DataSource = dataTable; // Привязываем данные к DataGridView
                return 0; // Успех
            }
            catch (Exception ex)
            {
                // В случае ошибки покажем сообщение об ошибке
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                return -1; // Ошибка
            }
            finally
            {
                dataBase.closedConnection(); // Закрываем соединение в любом случае
            }
        }
        // Добавление новой записи в базу данных
        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateInputs()) // Проверяем правильность введённых данных
            {
                string title = textBox1.Text; // Получаем название из TextBox
                double amount = double.Parse(textBox2.Text); // Получаем сумму
                string date = textBox3.Text; // Получаем дату
                string type = comboBox1.SelectedItem.ToString(); // Получаем тип (Доход или Расход)
                // SQL-запрос для вставки новой записи
                string insertQuery = "INSERT INTO Finance_db (Title, Amount, Date, Type) VALUES (@Title, @Amount, @Date, @Type)";
                sqlCommand = new SqlCommand(insertQuery, dataBase.getConnection());
                // Параметры для SQL-запроса
                sqlCommand.Parameters.AddWithValue("@Title", title);
                sqlCommand.Parameters.AddWithValue("@Amount", amount);
                sqlCommand.Parameters.AddWithValue("@Date", date);
                sqlCommand.Parameters.AddWithValue("@Type", type);
                try
                {
                    dataBase.openConnection(); // Открываем соединение
                    sqlCommand.ExecuteNonQuery(); // Выполняем запрос
                    LoadData(); // Перезагружаем данные в таблицу
                    ClearFields(); // Очищаем поля ввода
                }
                catch (Exception ex)
                {
                    // Показываем сообщение об ошибке, если что-то пошло не так
                    MessageBox.Show("Ошибка при добавлении записи: " + ex.Message);
                }
                finally
                {
                    dataBase.closedConnection(); // Закрываем соединение
                }
            }
        }
        // Редактирование существующей записи
        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли запись и корректны ли данные
            if (dataGridView1.SelectedRows.Count > 0 && ValidateInputs())
            {
                // Получаем ID записи, которую нужно редактировать
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                string title = textBox1.Text; // Получаем название
                double amount = double.Parse(textBox2.Text); // Получаем сумму
                string date = textBox3.Text; // Получаем дату
                string type = comboBox1.SelectedItem.ToString(); // Получаем тип
                // SQL-запрос для обновления записи
                string updateQuery = "UPDATE Finance_db SET Title=@Title, Amount=@Amount, Date=@Date, Type=@Type WHERE Id=@Id";
                sqlCommand = new SqlCommand(updateQuery, dataBase.getConnection());
                // Параметры для SQL-запроса
                sqlCommand.Parameters.AddWithValue("@Title", title);
                sqlCommand.Parameters.AddWithValue("@Amount", amount);
                sqlCommand.Parameters.AddWithValue("@Date", date);
                sqlCommand.Parameters.AddWithValue("@Type", type);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                try
                {
                    dataBase.openConnection(); // Открываем соединение
                    sqlCommand.ExecuteNonQuery(); // Выполняем запрос
                    LoadData(); // Перезагружаем данные в таблицу
                    ClearFields(); // Очищаем поля ввода
                }
                catch (Exception ex)
                {
                    // Показываем сообщение об ошибке, если что-то пошло не так
                    MessageBox.Show("Ошибка при редактировании записи: " + ex.Message);
                }
                finally
                {
                    dataBase.closedConnection(); // Закрываем соединение
                }
            }
            else
            {
                // Если запись не выбрана, выводим сообщение
                MessageBox.Show("Выберите запись для редактирования.");
            }
        }
        // Удаление выбранной записи
        private void button3_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли запись для удаления
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем ID записи для удаления
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                // SQL-запрос для удаления записи
                string deleteQuery = "DELETE FROM Finance_db WHERE Id=@Id";
                sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
                sqlCommand.Parameters.AddWithValue("@Id", id);
                try
                {
                    dataBase.openConnection(); // Открываем соединение
                    sqlCommand.ExecuteNonQuery(); // Выполняем запрос
                    LoadData(); // Перезагружаем данные в таблицу
                    ClearFields(); // Очищаем поля ввода
                }
                catch (Exception ex)
                {
                    // Показываем сообщение об ошибке, если что-то пошло не так
                    MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
                }
                finally
                {
                    dataBase.closedConnection(); // Закрываем соединение
                }
            }
            else
            {
                // Если запись не выбрана, выводим сообщение
                MessageBox.Show("Выберите запись для удаления.");
            }
        }
        // Очистка полей ввода
        private void ClearFields()
        {
            textBox1.Text = ""; // Очищаем поле с названием
            textBox2.Text = ""; // Очищаем поле с суммой
            textBox3.Text = ""; // Очищаем поле с датой
            comboBox1.SelectedIndex = -1; // Сбрасываем выбор в ComboBox
        }
        // Проверка правильности введённых данных
        private bool ValidateInputs()
        {
            // Проверка поля с названием
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Название не может быть пустым.");
                return false;
            }
            // Проверка корректности суммы
            if (!double.TryParse(textBox2.Text, out _))
            {
                MessageBox.Show("Введите корректное значение суммы.");
                return false;
            }
            // Проверка корректности даты
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Введите корректную дату.");
                return false;
            }
            // Проверка выбора типа (Доход или Расход)
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип записи: Доход или Расход.");
                return false;
            }
            return true; // Все данные корректны
        }
    }
}