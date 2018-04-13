using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Xml;


namespace TestTask
{
    class DeserializedObj
    {
        public string fileName { get; private set; }
        public int calculationsCount { get; private set; }
        public bool serializationCorrect { get; private set; }
        string filePath;
        List<Operands> operandList = new List<Operands>();
        List<int> modifiersList = new List<int>();


        public DeserializedObj(string filePath)
        {
            this.filePath = filePath;
            calculationsCount = 0;
        }


        public void StartDeserialization()
        {
            Deserealize(filePath);
        }


        public int PerformCalculations()
        {
            int result = 0;

            for (int i = 0; i < operandList.Count; i++)
            {
                result = Calculator.PerformOperation(result, operandList[i], modifiersList[i]);
            }
            return result;
        }


        void Deserealize(string file)
        {
            int calculationNumber = 1;//счётчик блоков calculation для вывода номера проблемного блока на консоль
            fileName = Path.GetFileNameWithoutExtension(file);
            try
            {
                XDocument xdoc = XDocument.Load(file);

                IEnumerable<XElement> calculations = xdoc.Elements("folder");
                IEnumerable<XElement> calculation = calculations.Elements("folder");
                foreach (XElement calc in calculation)
                {
                    bool correct = true;
                    int elementNumber = 1;

                    // проверка на наличие аттрибутов name и value 
                    foreach (XElement xe in calc.Elements())
                    {
                        XAttribute name = xe.Attribute("name");
                        XAttribute value = xe.Attribute("value");

                        if (name == null || value == null)
                        {
                            correct = false;
                            Console.WriteLine(
                                "Файл {0}:\nОшибка элемента в строке {1} элемента calculation № {2}.\nОтсутствует аттрибут.\n",
                                fileName, elementNumber, calculationNumber);
                        }
                        elementNumber++;
                    }
                    if (!correct)
                    {
                        continue;
                    }


                    IEnumerable<XElement> str = calc.Elements("str");
                    XElement val = calc.Element("int");
                    Operands op;
                    int intValue;
                    if (val.Attribute("name").Value == "mod" && int.TryParse(val.Attribute("value").Value, out intValue))
                    {
                        foreach (XElement elem in str)
                        {
                            string name = elem.Attribute("name").Value;
                            if (name == "operand" && Enum.TryParse(elem.Attribute("value").Value, out op))
                            {
                                operandList.Add(op);
                                modifiersList.Add(intValue);
                                calculationsCount++;
                            }
                        }
                    }
                    else { Console.WriteLine("Ошибка модификатора элемента calculation № {0} файла {1}", calculationNumber, fileName); }

                    calculationNumber++;
                }
                serializationCorrect = true;
            }
            catch (XmlException e)
            {
                Console.WriteLine("Ошибка десериализации файла: {0}\n" + e.Message + '\n', fileName);
                serializationCorrect = false;
            }
        }
    }
}

