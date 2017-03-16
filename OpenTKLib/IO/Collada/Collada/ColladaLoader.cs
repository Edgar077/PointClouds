using System;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;

namespace OpenTKExtension.Collada
{
    public static class ColladaLoader
	{
		private static XNamespace ns = "{http://www.collada.org/2005/11/COLLADASchema}";
        //<COLLADA xmlns="http://www.collada.org/2005/11/COLLADASchema" version="1.4.1">

        //private static XNamespace ns = "{https://www.khronos.org/files/collada_schema_1_5}";
                

        public static ColladaModel LoadFile(string fileName)
        {

            XDocument root = XDocument.Load(fileName);

            IEnumerable<XElement> elements= root.Elements();
            System.Diagnostics.Debug.Write(elements);


            List<XElement> listelements = elements.ToList<XElement>();

            IEnumerable<XNode> baseNodes = root.DescendantNodes();
            List<XNode> listbaseNodes = baseNodes.ToList<XNode>();





            IEnumerable<XElement> descendants = root.Descendants();
            List<XElement> listDescendants = descendants.ToList<XElement>();

           // for(int i = 0; i < descendants.GetEnumerator().MoveNext)
            System.Diagnostics.Debug.Write(root);

            ColladaModel model = new ColladaModel();

            // Parse Geometries
            

            var geoPaths = root.Descendants($"{ns}mesh");
            //geoPaths = root.Descendants(ns + "geometry");
            //geoPaths = root.Descendants($"{ns}mesh");

            //geoPaths = root.Descendants(ns + "mesh");
          //  var geoPaths = root.Descendants($"{ns}mesh");
            if (!geoPaths.Any())
                throw new ApplicationException("Failed to find geometries!");

            foreach (var geoPath in geoPaths)
            {
                var geoLoader = new PointCloudLoader(geoPath);
                var geometry = geoLoader.Load();

                model.PointClouds.Add(geometry);
            }

            // Parse Materials
            var matPaths = root.Descendants($"{ns}material");
            foreach (var matPath in matPaths)
            {
                var materialLoader = new MaterialLoader(root, matPath);
                var material = materialLoader.Load();

                model.Materials.Add(material);
            }


            return model;

        }
	}
}