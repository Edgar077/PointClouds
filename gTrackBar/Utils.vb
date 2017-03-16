Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing.Design


#Region "PointFConverter"

Friend Class PointFConverter : Inherits ExpandableObjectConverter

    Public Overloads Overrides Function CanConvertFrom( _
    ByVal context As ITypeDescriptorContext, _
    ByVal sourceType As Type) As Boolean

        If (sourceType Is GetType(String)) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)

    End Function

    Public Overloads Overrides Function ConvertFrom( _
    ByVal context As ITypeDescriptorContext, _
    ByVal culture As System.Globalization.CultureInfo, _
    ByVal value As Object) As Object

        If TypeOf value Is String Then
            Try
                Dim s As String = CType(value, String)
                Dim ConverterParts(2) As String
                ConverterParts = Split(s, ",")
                If Not IsNothing(ConverterParts) Then
                    If IsNothing(ConverterParts(0)) Then ConverterParts(0) = "-5"
                    If IsNothing(ConverterParts(1)) Then ConverterParts(1) = "-2.5"
                    Return New PointF(CSng(ConverterParts(0).Trim), _
                                      CSng(ConverterParts(1).Trim))
                End If
            Catch ex As Exception
                Throw New ArgumentException(String.Format("Can not convert '{0}' to type Corners", CStr(value)))
            End Try
        Else
            Return New PointF(-5.0F, -2.5F)
        End If

        Return MyBase.ConvertFrom(context, culture, value)

    End Function

    Public Overloads Overrides Function ConvertTo( _
    ByVal context As ITypeDescriptorContext, _
    ByVal culture As System.Globalization.CultureInfo, _
    ByVal value As Object, ByVal destinationType As Type) As Object

        If (destinationType Is GetType(String) _
        AndAlso TypeOf value Is PointF) Then

            Dim ConverterProperty As PointF = CType(value, PointF)
            ' build the string representation 
            Return String.Format("{0}, {1}", _
                                 ConverterProperty.X, _
                                 ConverterProperty.Y)
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)

    End Function

End Class 'PointFConverter Class

#End Region

#Region "ColorPack"

#Region "ColorPack Class"

Public Class ColorPack

    Public Sub New()
        _border = Color.DarkBlue
        _face = Color.Blue
        _highlight = Color.AliceBlue
    End Sub
    Public Sub New(ByVal Border As Color, ByVal Face As Color, ByVal Highlight As Color)
        _border = Border
        _face = Face
        _highlight = Highlight
    End Sub

    Private _border As Color = Color.Blue
    Public Property Border() As Color
        Get
            Return _border
        End Get
        Set(ByVal Value As Color)
            _border = Value
        End Set
    End Property

    Private _face As Color = Color.Blue
    Public Property Face() As Color
        Get
            Return _face
        End Get
        Set(ByVal Value As Color)
            _face = Value
        End Set
    End Property

    Private _highlight As Color = Color.AliceBlue
    Public Property Highlight() As Color
        Get
            Return _highlight
        End Get
        Set(ByVal Value As Color)
            _highlight = Value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return String.Format("{0};{1};{2}", _
                             getColorString(_border), _
                             getColorString(_face), _
                             getColorString(_highlight))
    End Function

    Private Function getColorString(ByVal bcolor As Color) As String
        If bcolor.IsNamedColor Then
            Return bcolor.Name
        Else
            Return String.Format("{0},{1},{2},{3}", bcolor.A, bcolor.R, bcolor.G, bcolor.B)
        End If
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Me.ToString = CType(obj, ColorPack).ToString
    End Function

End Class
#End Region

#Region "ColorPackConverter"

Friend Class ColorPackConverter : Inherits ExpandableObjectConverter

    Public Overrides Function GetCreateInstanceSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function CreateInstance(ByVal context As ITypeDescriptorContext, ByVal propertyValues As IDictionary) As Object
        Dim Item As New ColorPack
        Item.Border = CType(propertyValues("Border"), Color)
        Item.Face = CType(propertyValues("Face"), Color)
        Item.Highlight = CType(propertyValues("Highlight"), Color)
        Return Item
    End Function

    Public Overloads Overrides Function CanConvertFrom( _
    ByVal context As ITypeDescriptorContext, _
    ByVal sourceType As Type) As Boolean

        If (sourceType Is GetType(String)) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)

    End Function

    Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, _
    ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object


        If TypeOf value Is String Then
            Try
                Dim bColors As New List(Of Color)

                For Each cstring As String In Split(CStr(value), ";")
                    bColors.Add(CType(TypeDescriptor.GetConverter( _
                    GetType(Color)).ConvertFromString(cstring), Color))
                Next

                If Not IsNothing(bColors) AndAlso bColors.Count <> 3 Then
                    Throw New ArgumentException()
                Else
                    Return New ColorPack(bColors(0), bColors(1), bColors(2))
                End If
            Catch ex As Exception
                Throw New ArgumentException( _
                String.Format("Can not convert '{0}' to type ColorPack", _
                              CStr(value)))
            End Try

        Else
            Return New ColorPack()
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, _
    ByVal culture As System.Globalization.CultureInfo, _
    ByVal value As Object, ByVal destinationType As Type) As Object

        If (destinationType Is GetType(String) AndAlso TypeOf value Is ColorPack) Then
            Return CType(value, ColorPack).ToString
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)

    End Function

End Class

#End Region

#Region "ColorPackEditor"

Public Class ColorPackEditor
    Inherits UITypeEditor

    Public Overrides Function GetPaintValueSupported( _
    ByVal context As ITypeDescriptorContext) As Boolean

        Return True

    End Function

    Public Overrides Sub PaintValue(ByVal e As PaintValueEventArgs)
        ' Erase the area.
        e.Graphics.FillRectangle(Brushes.White, e.Bounds)

        Dim cPack As ColorPack
        If IsNothing(e.Context) Then
            cPack = New ColorPack
        Else
            cPack = CType(e.Value, ColorPack)
        End If
        ' Draw the sample.
        Using border_pen As Pen = New Pen(cPack.Border, 2), gp As New GraphicsPath
            gp.AddRectangle(e.Bounds)
            If e.Context.PropertyDescriptor.DisplayName = "AButColor" _
            OrElse (CType(e.Context.Instance, gTrackBar).BrushStyle = gTrackBar.eBrushStyle.Linear _
            OrElse CType(e.Context.Instance, gTrackBar).BrushStyle = gTrackBar.eBrushStyle.Linear2) Then
                Using br As LinearGradientBrush = New LinearGradientBrush(gp.GetBounds, _
                                                                          cPack.Highlight, cPack.Face, LinearGradientMode.Horizontal)

                    e.Graphics.FillPath(br, gp)

                End Using
            Else
                Using br As PathGradientBrush = New PathGradientBrush(gp)
                    br.SurroundColors = New Color() {cPack.Face}
                    br.CenterColor = cPack.Highlight
                    br.CenterPoint = New PointF(br.CenterPoint.X - 5, CSng(br.CenterPoint.Y - 2.5))
                    br.FocusScales = New PointF(0, 0)
                    e.Graphics.FillPath(br, gp)
                End Using
            End If

            e.Graphics.DrawRectangle(border_pen, 2, 2, e.Bounds.Width - 2, e.Bounds.Height - 2)
        End Using

    End Sub
End Class

#End Region
#End Region

#Region "ColorLinearGradient"

#Region "ColorLinearGradient Class"

Public Class ColorLinearGradient

    Public Sub New()
        _ColorA = Color.Blue
        _ColorB = Color.Black
    End Sub
    Public Sub New(ByVal ColorA As Color, ByVal ColorB As Color)
        _ColorA = ColorA
        _ColorB = ColorB
    End Sub

    Private _ColorA As Color = Color.Blue
    Public Property ColorA() As Color
        Get
            Return _ColorA
        End Get
        Set(ByVal Value As Color)
            _ColorA = Value
        End Set
    End Property

    Private _ColorB As Color = Color.Black
    Public Property ColorB() As Color
        Get
            Return _ColorB
        End Get
        Set(ByVal Value As Color)
            _ColorB = Value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return String.Format("{0};{1}", _
                             getColorString(_ColorA), _
                             getColorString(_ColorB))
    End Function

    Private Function getColorString(ByVal scolor As Color) As String
        If scolor.IsNamedColor Then
            Return scolor.Name
        Else
            Return String.Format("{0},{1},{2},{3}", scolor.A, scolor.R, scolor.G, scolor.B)
        End If
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Me.ToString = CType(obj, ColorLinearGradient).ToString
    End Function

End Class

#End Region

#Region "ColorLinearGradientConverter"

Friend Class ColorLinearGradientConverter : Inherits ExpandableObjectConverter

    Public Overrides Function GetCreateInstanceSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function CreateInstance(ByVal context As ITypeDescriptorContext, ByVal propertyValues As IDictionary) As Object
        Dim Item As New ColorLinearGradient
        Item.ColorA = CType(propertyValues("ColorA"), Color)
        Item.ColorB = CType(propertyValues("ColorB"), Color)
        Return Item
    End Function

    Public Overloads Overrides Function CanConvertFrom( _
    ByVal context As ITypeDescriptorContext, _
    ByVal sourceType As Type) As Boolean

        If (sourceType Is GetType(String)) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)

    End Function

    Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, _
    ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object


        If TypeOf value Is String Then
            Try
                Dim bColors As New List(Of Color)

                For Each cstring As String In Split(CStr(value), ";")
                    bColors.Add(CType(TypeDescriptor.GetConverter( _
                    GetType(Color)).ConvertFromString(cstring), Color))
                Next

                If Not IsNothing(bColors) AndAlso bColors.Count <> 2 Then
                    Throw New ArgumentException()
                Else
                    Return New ColorLinearGradient(bColors(0), bColors(1))
                End If
            Catch ex As Exception
                Throw New ArgumentException( _
                String.Format("Can not convert '{0}' to type ColorLinearGradient", _
                              CStr(value)))
            End Try

        Else
            Return New ColorLinearGradient()
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, _
    ByVal culture As System.Globalization.CultureInfo, _
    ByVal value As Object, ByVal destinationType As Type) As Object

        If (destinationType Is GetType(String) AndAlso TypeOf value Is ColorLinearGradient) Then
            Return CType(value, ColorLinearGradient).ToString
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)

    End Function

End Class

#End Region

#Region "ColorLinearGradientEditor"

Public Class ColorLinearGradientEditor
    Inherits UITypeEditor

    Public Overrides Function GetPaintValueSupported( _
    ByVal context As ITypeDescriptorContext) As Boolean

        Return True

    End Function

    Public Overrides Sub PaintValue(ByVal e As PaintValueEventArgs)
        ' Erase the area.
        e.Graphics.FillRectangle(Brushes.White, e.Bounds)

        Dim cLinearGradient As ColorLinearGradient
        If IsNothing(e.Context) Then
            cLinearGradient = New ColorLinearGradient
        Else
            cLinearGradient = CType(e.Value, ColorLinearGradient)
        End If
        ' Draw the sample.
        Using border_pen As Pen = New Pen(Color.Black, 1)
            Using br As LinearGradientBrush = New LinearGradientBrush(e.Bounds, _
                                                                      cLinearGradient.ColorA, cLinearGradient.ColorB, LinearGradientMode.Horizontal)

                e.Graphics.FillRectangle(br, e.Bounds)

            End Using

            e.Graphics.DrawRectangle(border_pen, 1, 1, e.Bounds.Width - 1, e.Bounds.Height - 1)
        End Using

    End Sub
End Class

#End Region

#End Region


