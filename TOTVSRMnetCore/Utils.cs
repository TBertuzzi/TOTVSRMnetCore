using System;
using System.Data;
using System.IO;
using System.Net;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace TOTVSRMnetCore
{
  /// <summary>
  /// Funções utilitárias
  /// </summary>
  public static class Utils
  {
    public static string LibPath = null;

    /// <summary>
    /// Carrega o assembly
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static Assembly LoadAssembly(string assemblyName)
    {
      return Assembly.Load(assemblyName);
    }

    /// <summary>
    /// Cria o binding do cliente do WS
    /// </summary>
    /// <returns></returns>
    public static Binding CreateBinding()
    {
      BasicHttpBinding binding = new BasicHttpBinding();
      binding.TextEncoding = System.Text.Encoding.UTF8;

      // configura o binding...
      binding.MaxBufferPoolSize = Int32.MaxValue;
      binding.MaxReceivedMessageSize = Int32.MaxValue;
      binding.ReaderQuotas = new XmlDictionaryReaderQuotas();
      binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
      binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
      binding.ReaderQuotas.MaxDepth = int.MaxValue;
      binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
      binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
      
      // configura o timeout...
      binding.CloseTimeout = TimeSpan.MaxValue;
      binding.OpenTimeout = TimeSpan.MaxValue;
      binding.ReceiveTimeout = TimeSpan.MaxValue;
      binding.SendTimeout = TimeSpan.MaxValue;
      
      // configura a segurança para receber o usuário e senha...
      binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
      binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
      return binding;
    }

    /// <summary>
    /// Carrega o DataSet a partir do XML
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DataSet LoadDataSet(string data)
    {
      // verifica se é um XML válido...
      if (!data.StartsWith("<"))
        throw new Exception(data);

      // cria o dataset...
      DataSet dataSet = new DataSet();
      dataSet.EnforceConstraints = false;

      // carrega os dados...
      LoadDataSet(dataSet, data);

      // aplica as alterações...
      dataSet.AcceptChanges();

      return dataSet;
    }

    public static void LoadDataSet(DataSet dataSet, string data)
    {
      // verifica se é um XML válido...
      if (!data.StartsWith("<", StringComparison.CurrentCultureIgnoreCase))
        throw new Exception(data);

      // faz a leitura do xml...
      dataSet.Clear();
      try
      {
        using (StringReader reader = new StringReader(data))
          dataSet.ReadXml(reader, XmlReadMode.Auto);
      }
      catch
      {
        dataSet.Clear();
        using (StringReader reader = new StringReader(data))
          dataSet.ReadXml(reader, XmlReadMode.IgnoreSchema);
      }
    }

    public static object ConvertToType(object value, Type dataType)
    {
      // verifica se precisa converter...
      if (value != null && dataType != null && dataType != typeof(object) && value.GetType() != dataType)
      {
        // tenta converter...
        try
        {
          value = Convert.ChangeType(value, dataType);
        }
        catch
        {
          // ignora...
        }
      }
      return value;
    }

    public static string GetDataSetXML(DataSet dataSet, bool includeSchema, 
      bool ignoreExtendedProps, bool encodeCDATA)
    {
      string xml = null;

      // clona o DataSet para remover os extended properties que não são necessários...
      using (DataSet xmlDataSet = dataSet.Copy())
      {
        // remove as propriedades extendidas...
        if (ignoreExtendedProps)
        {
          xmlDataSet.ExtendedProperties.Clear();
          foreach (DataTable table in xmlDataSet.Tables)
          {
            table.ExtendedProperties.Clear();
            foreach (DataColumn column in table.Columns)
              column.ExtendedProperties.Clear();
          }
        }

        using (StringWriter writer = new StringWriter())
        {
          XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
          xmlWriterSettings.OmitXmlDeclaration = true;
          xmlWriterSettings.Indent = true;
          xmlWriterSettings.IndentChars = "  ";
          xmlWriterSettings.NewLineHandling = NewLineHandling.Entitize;
          XmlWriter xmlWriter = XmlWriter.Create(writer, xmlWriterSettings);

          // grava os dados do DataSet...
          if (includeSchema)
            xmlDataSet.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);
          else
            xmlDataSet.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);

          // pega o xml...
          writer.Flush();
          xml = writer.ToString();
        }
      }

      // codifica como CDATA...
      if (encodeCDATA)
        xml = System.Web.HttpUtility.HtmlEncode(xml);
      return xml;
    }

    /// <summary>
    /// Converte um array de bytes em uma instância de um objeto
    /// </summary>
    /// <param name="bytes">Array a ser convertido</param>
    /// <param name="instanceType">Tipo da instância a ser retornada</param>
    /// <returns></returns>
    public static object BytesToObject(byte[] bytes, Type instanceType)
    {
      // cria o stream de serialização...
      using (MemoryStream ms = new MemoryStream(bytes))
      {
        // deserializa...
        IFormatter formatter = new BinaryFormatter(teste, new StreamingContext());
        object result = formatter.Deserialize(ms);

        // retorna o objeto deserializado...
        return result;
      }
    }

    private static readonly Teste teste = new Teste();

    public class Teste : ISurrogateSelector
    {
      readonly SurrogateSelector surrogate = new SurrogateSelector();
      public void ChainSelector(ISurrogateSelector selector)
      {
        try
        {
          surrogate.ChainSelector(selector);
        }
        catch (Exception exception)
        {

        }
      }

      public ISurrogateSelector GetNextSelector()
      {
        try
        {
          return surrogate.GetNextSelector();
        }
        catch
        {
          return this;
        }
      }

      public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
      {
        try
        {
          return surrogate.GetSurrogate(type, context, out selector);
        }
        catch
        {
          selector = this;
          return null;
        }
      }
    }
  }
}
