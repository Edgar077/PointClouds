using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;

using System.IO;
using Newtonsoft.Json;
using OpenTK;

namespace UnitTestsOpenTK.Characters
{
    [TestFixture]
    [Category("UnitTest")]
    public class JsonTester : TestBaseICP
    {
         
        
        [Test]
        public void SerializeListVectors()
        {

            CharacterCreator.tester t = new CharacterCreator.tester();
            t.ListVectors();


        }
        [Test]
        public void SerializeListFloats()
        {

            CharacterCreator.tester t = new CharacterCreator.tester();
            t.SerializeListFloats();


        }

       

    }
}
