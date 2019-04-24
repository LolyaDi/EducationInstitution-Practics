using EducationInstitution.DataAccess;
using EducationInstitution.Models;
using System.Collections.Generic;
using System.Data;

namespace EducationInstitution.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dataSet = new DataSet("EducationInstitution");

            string userChoice;
            int userNumber;
            bool isParsed;

            System.Console.WriteLine("К какой таблице желаете получить доступ?" +
                "\n1) Students");

            userChoice = System.Console.ReadLine();
            
            isParsed = int.TryParse(userChoice, out userNumber);
            
            if(!isParsed)
            {
                System.Console.WriteLine("Были введены некорректные символы!");
                return;
            }

            switch (userNumber)
            {
                case 1:
                    DataAccessLayer<Student> dataAccess = new DataAccessLayer<Student>();

                    dataAccess.Connect();

                    dataAccess.Dataset = dataSet;

                    dataAccess.GainAccess("Students");

                    System.Console.WriteLine("Выберите действие:" +
                        "\n1) Добавить студента." +
                        "\n2) Удалить студента." +
                        "\n3) Изменить данные студента." +
                        "\n4) Вывести информацию обо всех студентах заведения.");

                    userChoice = System.Console.ReadLine();
                    
                    isParsed = int.TryParse(userChoice, out userNumber);

                    if (!isParsed)
                    {
                        System.Console.WriteLine("Были введены некорректные символы!");
                        return;
                    }

                    var students = dataAccess.SelectAllRows("Students");

                    switch (userNumber)
                    {
                        case 1:
                            InsertStudent(dataAccess);
                            break;
                        case 2:
                            GetAllStudents(dataAccess, students);
                            DeleteStudent(dataAccess, students.Count);  
                            break;
                        case 3:
                            GetAllStudents(dataAccess, students);
                            UpdateStudent(dataAccess, students.Count);
                            break;
                        case 4:
                            GetAllStudents(dataAccess, students);
                            break;
                        default:
                            System.Console.WriteLine("Введенное Вами число не соответствует ни одному из вышеперечисленных!");
                            break;
                    }

                    dataAccess.Disconnect();
                    break;
                default:
                    System.Console.WriteLine("Введенное Вами число не соответствует ни одному из вышеперечисленных!");
                    break;
            }

            System.Console.ReadLine();
        }

        private static void GetAllStudents(DataAccessLayer<Student> dataAccess, List<Student> students)
        {
            if (students.Count == 0)
            {
                System.Console.WriteLine("Информации о студентах нет!");
                return;
            }

            int i = 0;

            System.Console.WriteLine(string.Format("{0}) {1,-20}","№", "ФИО"));
            foreach (var student in students)
            {
                System.Console.WriteLine(string.Format("{0}) {1,-20}", ++i, student.Fullname));
            }
        }

        private static void UpdateStudent(DataAccessLayer<Student> dataAccess, int count)
        {
            if(count == 0)
            {
                System.Console.WriteLine("Обновление данных невозможно!");
                return;
            }

            string userString;
            bool isParsed;
            int id;
            
            System.Console.WriteLine("Введите номер студента, информацию которого хотите изменить:");
            userString = System.Console.ReadLine();

            isParsed = int.TryParse(userString, out id);

            if (!isParsed || id <= 0 || id > count)
            {
                System.Console.WriteLine("Неверный ввод!");
                return;
            }

            System.Console.WriteLine("Введите ФИО студента:");
            userString = System.Console.ReadLine();

            Student anotherStudent = new Student()
            {
                Fullname = userString
            };

            dataAccess.UpdateRow("Students", anotherStudent, --id);

            dataAccess.UpdateData("Students");

            System.Console.WriteLine("Информация о студенте была успешно обновлена!");
        }

        public static void InsertStudent(DataAccessLayer<Student> dataAccess)
        {
            System.Console.WriteLine("Введите ФИО студента:");
            string userString = System.Console.ReadLine();

            var student = new Student()
            {
                Fullname = userString
            };

            dataAccess.InsertRow("Students", student);

            dataAccess.UpdateData("Students");

            System.Console.WriteLine("Информация о студенте была успешно добавлена!");
        }

        public static void DeleteStudent(DataAccessLayer<Student> dataAccess, int count)
        {
            if (count == 0)
            {
                System.Console.WriteLine("Удаление данных невозможно!");
                return;
            }

            bool isParsed;
            int id;

            System.Console.WriteLine("Введите номер студента, которого хотите удалить:");
            string idString = System.Console.ReadLine();

            isParsed = int.TryParse(idString, out id);

            if (!isParsed || id <= 0 || id > count)
            {
                System.Console.WriteLine("Неверный ввод!");
                return;
            }

            dataAccess.DeleteRow("Students", --id);

            dataAccess.UpdateData("Students");

            System.Console.WriteLine("Информация о студенте была успешно удалена!");
        }
    }
}
