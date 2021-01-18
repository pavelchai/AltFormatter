/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Formatter;
using MicroBenchmark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace AltFormatter.Examples
{
    public static class SerializationReport<T>
    {
        private static IDictionary<string, Func<T, byte[]>> serializers;

        private static IDictionary<string, Func<byte[], T>> deserializers;

        static SerializationReport()
        {
            Assembly[] assemblies = { typeof(SerializationReport<>).Assembly };
            IFormatter formatterZipXMLTextWithoutCompression = new AltFormatterZipXmlText(false, assemblies);
            IFormatter formatterZipXMLTextCompression = new AltFormatterZipXmlText(true, assemblies);
            IFormatter formatterBinaryTextWithoutCompression = new AltFormatterBinaryText(false, assemblies);
            IFormatter formatterBinaryTextWithCompression = new AltFormatterBinaryText(true, assemblies);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            XmlSerializer xmlFormatter = new XmlSerializer(typeof(T));
            SoapFormatter soapFormatter = new SoapFormatter();

            serializers = new Dictionary<string, Func<T, byte[]>>(5);
            serializers.Add("AltFormatter (ZIP-64, XML + Text, Store)", v => formatterZipXMLTextWithoutCompression.Serialize<T>(v));
            serializers.Add("AltFormatter (ZIP-64, XML + Text, Deflate+Store)", v => formatterZipXMLTextCompression.Serialize<T>(v));
            serializers.Add("AltFormatter (Binary, Text, Store)", v => formatterBinaryTextWithoutCompression.Serialize<T>(v));
            serializers.Add("AltFormatter (Binary, Text, Deflate+Store)", v => formatterBinaryTextWithCompression.Serialize<T>(v));

            serializers.Add("Binary",
                v =>
                {
                    using (var ms = new MemoryStream())
                    {
                        binaryFormatter.Serialize(ms, v);
                        return ms.ToArray();
                    }
                });

            serializers.Add("XML",
                v =>
                {
                    using (var ms = new MemoryStream())
                    {
                        xmlFormatter.Serialize(ms, v);
                        return ms.ToArray();
                    }
                });

            serializers.Add("SOAP",
                v =>
                {
                    using (var ms = new MemoryStream())
                    {
                        soapFormatter.Serialize(ms, v);
                        return ms.ToArray();
                    }
                });

            deserializers = new Dictionary<string, Func<byte[], T>>(5);
            deserializers.Add("AltFormatter (ZIP-64, XML + Text, Store)", v => formatterZipXMLTextWithoutCompression.Deserialize<T>(v));
            deserializers.Add("AltFormatter (ZIP-64, XML + Text, Deflate+Store)", v => formatterZipXMLTextCompression.Deserialize<T>(v));
            deserializers.Add("AltFormatter (Binary, Text, Store)", v => formatterBinaryTextWithoutCompression.Deserialize<T>(v));
            deserializers.Add("AltFormatter (Binary, Text, Deflate+Store)", v => formatterBinaryTextWithCompression.Deserialize<T>(v));

            deserializers.Add("Binary",
                v =>
                {
                    using (var ms = new MemoryStream(v))
                    {
                        return (T)binaryFormatter.Deserialize(ms);
                    }
                });

            deserializers.Add("XML",
                v =>
                {
                    using (var ms = new MemoryStream(v))
                    {
                        return (T)xmlFormatter.Deserialize(ms);
                    }
                });

            deserializers.Add("SOAP",
                v =>
                {
                    using (var ms = new MemoryStream(v))
                    {
                        return (T)soapFormatter.Deserialize(ms);
                    }
                });
        }

        public static void GenerateReport(string name, Func<T> factory, TextWriter writer)
        {
            T value = factory();

            Func<T, byte[]> serializer;
            Func<byte[], T> deserializer;
            byte[] data;
            BenchmarkResult serializationResult;
            BenchmarkResult deserializationResult;

            OutputTable table = new OutputTable();
            table.SetHeaders("Serializer", "Serialization time (in s)", "Deserialization time (in s)", "Size (in bytes)");

            foreach (var pair in serializers)
            {
                serializer = pair.Value;
                deserializer = deserializers[pair.Key];
                data = serializer(value);

                serializationResult = Benchmark.Run(() => serializer(value), N: 45, NSkip: 5);
                deserializationResult = Benchmark.Run(() => deserializer(data), N: 45, NSkip: 5);

                table.AddRow(
                    pair.Key,
                    string.Format("{0:E3} +- {1:E3}", serializationResult.Mean, serializationResult.StdDev),
                    string.Format("{0:E3} +- {1:E3}", deserializationResult.Mean, deserializationResult.StdDev),
                    data.Length.ToString()
                    );
            }

            writer.WriteLine(string.Format("Serialization/Deserialization: {0}", name));
            writer.WriteLine(table.ToString());
        }
    }
}
