using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            //1. Принимаю путь до директории
            string directory = @Console.ReadLine();
            sw.Start();

            string[] files = Directory.GetFiles(directory, "*.xml");


            DeserializedObj[] deserialized = new DeserializedObj[files.Length];
            Action[] act = new Action[files.Length];

            int calculationLead = 0;
            string calcLeadFile = "";
            //2. десериализую данные из всех файлов .xml в указанной директории (3. operand в Enum 4. в случае ошибок пропускаю calculation)
            //1* считываю несколько файлов в разных потоках при помощи класса Parallel
            for (int i = 0; i < deserialized.Length; i++)
            {
                deserialized[i] = new DeserializedObj(files[i]);
                act[i] = deserialized[i].StartDeserialization;
            }

            Parallel.Invoke(act);

            //5. Для каждого файла произвожу последовательно арифметические действия в 
            //соответствии со считанными данными и вывожу в консоль имя файла и результат вычисления.
            foreach (DeserializedObj ds in deserialized)
            {
                if (ds.serializationCorrect)
                {
                    int result = ds.PerformCalculations();
                    Console.WriteLine("File: {0}\nResult: {1}\n", ds.fileName, result);
                    if (calculationLead < ds.calculationsCount)
                    {
                        calculationLead = ds.calculationsCount;
                        calcLeadFile = ds.fileName;
                    }
                }
            }

            sw.Stop();

            //В конце выполнения вывожу вывожу в консоль название файла с 
            //наибольшим количеством успешно десериализованных элементов "calculation" и время выполнения программы.
            Console.WriteLine("Файл с наибольшим количеством уcпешно десериализованных элементов calculation: \n{0}\n", calcLeadFile);
            Console.WriteLine("Время работы программы: {0} мс", sw.ElapsedMilliseconds);
            Console.ReadKey();

        }
    }
}


