using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework3
{
    class CreatHuffmanTree
    {
        List<TNode> nodeList;
        //CreatHuffman metodu ile agac olusturduktan sonra nodeList[0] root olarak kullanilacaktir. 

        //Karakterlerin frekanslarini hesaplar
        public void CalFrequency(string str)
        {
            nodeList = new List<TNode>();
            for (int i = 0; i < str.Length; i++)
            {
                bool flag = false; //Karater listte daha onceden eklemisse true
                foreach (TNode item in nodeList)
                {
                    if (item.Character == str[i])
                    {
                        item.Frequency++;
                        flag = true;
                        break;
                    }
                }
                if (flag == false) //Karakter daha onceden eklenmemişse
                    nodeList.Add(new TNode(str[i], 1));
            }
        }

        //Karakterlerden Huffman agaci olusturur
        public void CreatHuffman(string str)
        {
            if (nodeList == null)
                CalFrequency(str); //Frekans hesaplama

            while (nodeList.Count > 1)
            {
                for (int i = 1; i < nodeList.Count; i++) //Listte her defasinda en kucuk 2 elamani basa ceker
                {
                    if (nodeList[i].Frequency < nodeList[0].Frequency)
                    {
                        TNode temp = nodeList[0];
                        nodeList[0] = nodeList[i];
                        nodeList[i] = temp;
                        continue;
                    }
                    if (nodeList[i].Frequency < nodeList[1].Frequency && nodeList[i].Frequency >= nodeList[0].Frequency)
                    {
                        TNode temp = nodeList[1];
                        nodeList[1] = nodeList[i];
                        nodeList[i] = temp;
                    }
                }

                TNode newNode = new TNode();
                newNode.Left = nodeList[0];
                nodeList.Remove(nodeList[0]); //Ilk elemani sileriz, yerine bir sonraki gelir
                newNode.Right = nodeList[0];
                nodeList.Remove(nodeList[0]);
                //newNode da birlestirilen karakterleri kullanmayacagimiz icin hesaplamamiza gerek yoktur
                newNode.Frequency = newNode.Left.Frequency + newNode.Right.Frequency; //newNode un frekansi belirlenir
                nodeList.Add(newNode); //newNode liste eklenir
            }
            
        }

        //Karakterler ve frekanslarini goseterir.
        public void DisplayFrequency()
        {
            if (nodeList != null)
            {
                Console.WriteLine("\nCharacter" + "\t" + "Frequency");
                foreach (TNode item in nodeList)
                {
                    if ((int)item.Character == 13)
                        continue;
                    if ((int)item.Character == 10)
                    {
                        Console.WriteLine("\\n" + "\t\t" + item.Frequency);
                        continue;
                    }
                    Console.WriteLine(item.Character + "\t\t" + item.Frequency);
                    /*Bir satir asagi gecme 2 karakter icerir. Bunlarin kodu 13 ve 10 dur. 
                     Frekans listesinin anlasilir ve duzenli olabilmesi icin if lerle durumu kontrol edip,
                     satir atlamayi \n ile gosterdim.*/
                }
            }
        }    

        //Gelen kodlari olusturulmus agaca gore cozumler.
        public string Decoded(string code)
        {
            string result = "";
            TNode iterator = nodeList[0];
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == '0')
                {
                    iterator = iterator.Left;
                    if (iterator.Left == null && iterator.Right == null)//Yaprak olma durumu kontrol edilir.
                    {
                        result = result + iterator.Character;
                        iterator = nodeList[0];
                    }
                }
                else if (code[i] == '1')
                {
                    iterator = iterator.Right;
                    if (iterator.Left == null && iterator.Right == null)
                    {
                        result = result + iterator.Character;
                        iterator = nodeList[0];
                    }
                }
            }
            if (iterator != nodeList[0])//Kalan kodlar iteratoru yapraga goturemiyorsa kodlar bu agaca ait degildir.
                return "Kodlar var olan Huffman agaci ile uyusmamaktadir!!!";
            return result;
        }

        //Olusturulan agactaki karakterin kodunu donderir.
        public string Encode(char character, TNode node, string code)
        {
            string result;
            if (node.Left == null && node.Right == null) //Karakterler sadece yapraklarada bulunur
            {
                if (character == node.Character)
                    result = code;
                else
                    result = null;
            }
            else
            {
                result = Encode(character, node.Left, code + "0");
                if (result == null) 
                    result = Encode(character, node.Right, code + "1");
            }
            return result;
        }
        //String deki karakterleri kodlarini hesaplar
        public string Encode(string str)
        {
            string result = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (Encode(str[i], nodeList[0], "") == null)
                    return "Source, Letter da bulunmayan karakter iceriyor!!!";
                result = result + Encode(str[i], nodeList[0], "");
            }
            return result;
        }

        public double CompressionRatio(string str, string code)
        {
            return (str.Length) * 8 / (code.Length);
        }

        //letter.txt islemlerini yapar (Odev madde 1 2 3 4)
        public void OperationLettertxt()
        {
            StreamReader fileRead = File.OpenText("letter.txt");
            string str = fileRead.ReadToEnd();
            fileRead.Close();
            CalFrequency(str);
            Console.WriteLine("\nLetter.txt icerisindeki karakterlerin frekanslari hesaplandi.");
            DisplayFrequency();
            CreatHuffman(str);
            Console.WriteLine("\nLetter.txt icerisindeki karakterler Huffman agacina yerlestirildi.");    
        }

        //source.txt, encoded.txt islemlerini yapar (Odev madde 5)
        public void OperationSourcetxt()
        {
            StreamReader fileRead = File.OpenText("source.txt");
            string str = fileRead.ReadToEnd();
            Console.WriteLine("\nVeriler Huffman agacina yerlestirildi.");
            StreamWriter fileWriter = new StreamWriter("encoded.txt");
            fileWriter.WriteLine(Encode(str));
            Console.WriteLine("\nVeriler encoded.txt dosyasina girildi.");
            fileWriter.Close();
            fileRead.Close();
        }
        
        //codes.txt, decodes.txt islemlerini yapar (Odev madde 6)
        public void OperationCodestxt()
        {
            StreamReader fileRead = File.OpenText("codes.txt");
            string str = fileRead.ReadToEnd();
            fileRead.Close();
            StreamWriter fileWriter = new StreamWriter("decoded.txt");
            fileWriter.WriteLine(Decoded(str));
            Console.WriteLine("\nVeriler decoded.txt dosyasina girildi.");
            fileWriter.Close();
        }

    }
}
