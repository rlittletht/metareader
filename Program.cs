// See https://aka.ms/new-console-template for more information

using Jp2000;

Console.WriteLine("Hello, World!");

FileStream stream = new FileStream("c:\\temp\\image10.jp2", FileMode.Open);

Metadata.CreateFromStream(stream);
