using NetTopologySuite.Geometries;
using ProjNet;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public static class GeometryExtensions
    {
        private static readonly CoordinateSystemServices _coordinateSystemServices
            = new CoordinateSystemServices(
                new Dictionary<int, string>
                {
                // Coordinate systems:

                [4326] = GeographicCoordinateSystem.WGS84.WKT,

                // This coordinate system covers the area of our data.
                // Different data requires a different coordinate system.
                [2830] =
                        @"
                    PROJCS[""NAD83(HARN) / New York West"",
    GEOGCS[""NAD83(HARN)"",
        DATUM[""NAD83_High_Accuracy_Reference_Network"",
            SPHEROID[""GRS 1980"",6378137,298.257222101,
                AUTHORITY[""EPSG"",""7019""]],
            TOWGS84[0,0,0,0,0,0,0],
            AUTHORITY[""EPSG"",""6152""]],
        PRIMEM[""Greenwich"",0,
            AUTHORITY[""EPSG"",""8901""]],
        UNIT[""degree"",0.0174532925199433,
            AUTHORITY[""EPSG"",""9122""]],
        AUTHORITY[""EPSG"",""4152""]],
    PROJECTION[""Transverse_Mercator""],
    PARAMETER[""latitude_of_origin"",40],
    PARAMETER[""central_meridian"",-78.58333333333333],
    PARAMETER[""scale_factor"",0.9999375],
    PARAMETER[""false_easting"",350000],
    PARAMETER[""false_northing"",0],
    UNIT[""metre"",1,
        AUTHORITY[""EPSG"",""9001""]],
    AXIS[""X"",EAST],
    AXIS[""Y"",NORTH],
    AUTHORITY[""EPSG"",""2830""]]

                "
                });

        public static Geometry ProjectTo(this Geometry geometry, int srid)
        {
            var transformation = _coordinateSystemServices.CreateTransformation(geometry.SRID, srid);

            var result = geometry.Copy();
            result.Apply(new MathTransformFilter(transformation.MathTransform));

            return result;
        }

        private class MathTransformFilter : ICoordinateSequenceFilter
        {
            private readonly MathTransform _transform;

            public MathTransformFilter(MathTransform transform)
                => _transform = transform;

            public bool Done => false;
            public bool GeometryChanged => true;

            public void Filter(CoordinateSequence seq, int i)
            {
                var x = seq.GetX(i);
                var y = seq.GetY(i);
                var z = seq.GetZ(i);
                _transform.Transform(ref x, ref y, ref z);
                seq.SetX(i, x);
                seq.SetY(i, y);
                seq.SetZ(i, z);
            }
        }
    }

}
