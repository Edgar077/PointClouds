using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;

using System.Diagnostics;

namespace UnitTestsOpenTK.KDTreeTest
{
    [TestFixture]
    [Category("UnitTest")]
	public class BoundingSphereTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Intersects
		///</summary>
		  [Test]
		public void IntersectsTest()
		{
			BoundingSphere target = new BoundingSphere(new Vector3(0,0,0), 10.0f);
			BoundingBoxAxisAligned boundBox = new BoundingBoxAxisAligned(new Vector3(-5, -5, -5), new Vector3(5, 5, 5));
			IntersectionTypes intersection_type = IntersectionTypes.SOLID;
			float slight_offset = 0.001f;

			bool expected = true;
			bool actual = target.Intersects(boundBox, intersection_type);
			Assert.AreEqual(expected, actual);

			{	// Test1: Solid intersections (IE: Sphere and AABB can reside inside of each other and it counts as an intersection)
				intersection_type = IntersectionTypes.SOLID;

				target = new BoundingSphere(new Vector3(0, 0, 0), 1.0f);	//Sphere inside AABB
				expected = true;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);

				target = new BoundingSphere(new Vector3(0, 0, 0), 1000.0f);	//AABB inside sphere
				expected = true;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);

				target = new BoundingSphere(new Vector3(6, 6, 6),  (boundBox.MaxCorner - new Vector3(6, 6, 6)).Length + slight_offset);	//Sphere crosses max corner of AABB
				expected = true;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);

				target = new BoundingSphere(new Vector3(6, 6, 6), (boundBox.MaxCorner - new Vector3(6, 6, 6)).Length - slight_offset);	//Sphere does not cross max corner of AABB
				expected = false;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);
			}

			{	// Test2: Hollow intersections (IE: Sphere and AABB only intersect one is partially outside of the other. As in when just their surfaces cross.)
				intersection_type = IntersectionTypes.HOLLOW;

				target = new BoundingSphere(new Vector3(0, 0, 0), 1.0f);
				expected = false;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);

				target = new BoundingSphere(new Vector3(0, 0, 0), 1000.0f);
				expected = false;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);

                target = new BoundingSphere(new Vector3(6, 6, 6), (boundBox.MaxCorner - new Vector3(6, 6, 6)).Length + slight_offset);	//Sphere crosses max corner of AABB
				expected = true;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);

				target = new BoundingSphere(new Vector3(6, 6, 6),  (boundBox.MaxCorner - new Vector3(6, 6, 6)).Length - slight_offset);	//Sphere does not cross max corner of AABB
				expected = false;
				actual = target.Intersects(boundBox, intersection_type);
				Assert.AreEqual(expected, actual);
			}

			{	//Test3: Intersects other bounding spheres
				BoundingSphere target2 = new BoundingSphere(new Vector3(0, 0, 0), 1.0f);
				target = new BoundingSphere(new Vector3(0, 0, 0), 1.0f);

				expected = true;
				actual = target.Intersects(target2, intersection_type);
				Assert.AreEqual(expected, actual);

				target2 = new BoundingSphere(new Vector3(10,10,10), 1.0f);
				target = new BoundingSphere(new Vector3(0, 0, 0), 1.0f);

				expected = false;
				actual = target.Intersects(target2, intersection_type);
				Assert.AreEqual(expected, actual);

				target2 = new BoundingSphere(new Vector3(10, 10, 10), 1000.0f);
				target = new BoundingSphere(new Vector3(0, 0, 0), 1.0f);

				expected = true;
				actual = target.Intersects(target2, intersection_type);
				Assert.AreEqual(expected, actual);

				expected = true;
				actual = target2.Intersects(target, intersection_type);
				Assert.AreEqual(expected, actual);
			}

		}
	}
}
