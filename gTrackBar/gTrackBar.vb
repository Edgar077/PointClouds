Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing.Design

''' <summary>
''' Customizable TrackBar Control
''' </summary>
''' <remarks>v1.9.0</remarks>

<ToolboxItem(True), ToolboxBitmap(GetType(gTrackBar), "VBControls.gTrackBar.bmp")> _
<DefaultEvent("ValueChanged")> _
<System.Diagnostics.DebuggerStepThrough()> _
Public Class gTrackBar
    Private WithEvents MouseTimer As New Timer
    Public Event ValueChanged(ByVal sender As Object, ByVal e As EventArgs)

#Region "Initiate"

    Private MouseState As eMouseState = eMouseState.Up
    Private IsOverSlider As Boolean
    Private IsOverDownButton As Boolean
    Private IsOverUpButton As Boolean
    Private ReadOnly gpSlider As New GraphicsPath()
    Private intSlideIndent As Integer = 13
    Private sngSliderPos As Single = 35
    Private rectValueBox As Rectangle = New Rectangle(0, 0, 30, 20)
    Private rectSlider As Rectangle = New Rectangle(0, 0, 250, 21)
    Private rectDownButton As Rectangle = New Rectangle(0, 2, 15, 26)
    Private rectUpButton As Rectangle = New Rectangle(235, 2, 15, 26)
    Private rectLabel As Rectangle = New Rectangle(0, 0, Width, 20)
    Private Init As Boolean = True

    Private ReadOnly sf As New StringFormat()

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Private Sub TBSlider_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        UpdateRects()
        Init = False

    End Sub

#End Region

#Region "Enum"

    Enum eTickType
        None
        Up_Right
        Down_Left
        Both
        Middle
    End Enum

    Enum eMouseState
        Up
        Down
    End Enum

    Enum eShape
        Ellipse
        Rectangle
        ArrowUp
        ArrowDown
        ArrowRight
        ArrowLeft
    End Enum

    Enum eValueBox
        None
        Left
        Right
    End Enum

    Enum eBrushStyle
        Image
        Linear
        Linear2
        Path
    End Enum

#End Region

#Region "Properties"

#Region "Hidden"

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Shared Shadows Property BorderStyle() As Boolean
        Get
            Return False 'always false 
        End Get
        Set(ByVal value As Boolean) 'empty 
        End Set
    End Property

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Shared Shadows Property Font() As Font
        Get
            Return Nothing 'always false 
        End Get
        Set(ByVal value As Font) 'empty 
        End Set
    End Property

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Shared Shadows Property ForeColor() As Color
        Get
            Return Nothing 'always false 
        End Get
        Set(ByVal value As Color) 'empty 
        End Set
    End Property

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Shadows Property Padding() As Padding
        Get
            Return _LabelPadding
        End Get
        Set(ByVal value As Padding)
            MyBase.Padding = value
        End Set
    End Property
#End Region

#Region "Value"

    Private _Value As Integer
    <Category("Appearance gTrackBar")> _
    <Description("Current Value for the Slider")> _
    <Bindable(True)> _
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If _Value <> value Then
                If value < _MinValue Then
                    _Value = _MinValue
                Else
                    If value > _MaxValue Then
                        _Value = _MaxValue
                    Else
                        _Value = value
                    End If
                End If
                UpdateRects()
                Invalidate()
                RaiseEvent ValueChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property

    <Category("Appearance gTrackBar")> _
    <Description("Current Value Adjusted by the Divisor")> _
    <Browsable(False)> _
    Public Property ValueAdjusted() As Single
        Get
            Return CSng(_Value / _valueDivisor)
        End Get
        Set(ByVal Val As Single)
            Value = CInt(Val * _valueDivisor)

        End Set
    End Property

    Enum eValueDivisor
        e1 = 1
        e10 = 10
        e100 = 100
        e1000 = 1000
    End Enum

    Private _valueDivisor As eValueDivisor = eValueDivisor.e1
    <Category("Appearance gTrackBar")> _
    <Description("Divisor to adjust the Value by")> _
    Public Property ValueDivisor() As eValueDivisor
        Get
            Return _valueDivisor
        End Get
        Set(ByVal Value As eValueDivisor)
            _valueDivisor = Value
        End Set
    End Property

    Private _valueStrFormat As String
    <Category("Appearance gTrackBar")> _
    <Description("Format to display the Value")> _
    Public Property ValueStrFormat() As String
        Get
            Return _valueStrFormat
        End Get
        Set(ByVal Value As String)
            _valueStrFormat = Value
            Invalidate()
        End Set
    End Property
#End Region

#Region "Control"

    Private _BrushStyle As eBrushStyle = eBrushStyle.Path
    <Category("Appearance Slider")> _
    <Description("Use a Linear or Path type Brush on the Slider")> _
    <DefaultValue(GetType(eBrushStyle), "Path")> _
    Public Property BrushStyle() As eBrushStyle
        Get
            Return _BrushStyle
        End Get
        Set(ByVal value As eBrushStyle)
            _BrushStyle = value
            Invalidate()
        End Set
    End Property

    Private _BrushDirection As LinearGradientMode = LinearGradientMode.Horizontal
    <Category("Appearance Slider")> _
    <Description("The LinearGradientMode for the Linear Fill Type Brush")> _
    <DefaultValue(GetType(LinearGradientMode), "Horizontal")> _
    Public Property BrushDirection() As LinearGradientMode
        Get
            Return _BrushDirection
        End Get
        Set(ByVal value As LinearGradientMode)
            _BrushDirection = value
            Invalidate()
        End Set
    End Property

    Private _Orientation As Orientation = Orientation.Horizontal
    <Category("Appearance gTrackBar")> _
    <Description("Horizontal or Vertical Orientation")> _
    <DefaultValue(GetType(Orientation), "Horizontal")> _
    Public Property Orientation() As Orientation
        Get
            Return _Orientation
        End Get
        Set(ByVal value As Orientation)
            _Orientation = value
            Size = New Size(Height, Width)
            SliderSize = New Size(_SliderSize.Height, _SliderSize.Width)
            UpdateRects()
            Invalidate()
        End Set
    End Property

    Private _MinValue As Integer
    <Category("Appearance gTrackBar")> _
    <Description("Minimum Value allowed for the Slider")> _
    <RefreshProperties(RefreshProperties.All)> _
    <DefaultValue(0)> _
    Public Property MinValue() As Integer
        Get
            Return _MinValue
        End Get
        Set(ByVal value As Integer)
            If Not Init Then
                If value >= _MaxValue Then value = _MaxValue - 10
                If _Value < value Then _Value = value
            End If
            _MinValue = value
            UpdateRects()
            Invalidate()
        End Set
    End Property

    Private _MaxValue As Integer = 50
    <Category("Appearance gTrackBar")> _
    <Description("Maximum Value allowed for the Slider")> _
    <RefreshProperties(RefreshProperties.All)> _
    <DefaultValue(50)> _
    Public Property MaxValue() As Integer
        Get
            Return _MaxValue
        End Get
        Set(ByVal value As Integer)
            If Not Init Then
                If value <= _MinValue Then value = _MinValue + 10
                If _Value > value Then _Value = value
            End If
            _MaxValue = value
            UpdateRects()
            Invalidate()
        End Set
    End Property

    Private _ChangeLarge As Integer = 10
    <Category("Appearance gTrackBar")> _
    <Description("How far to adjust the value when clicking to the right or left of the slider or when the Arrow Keys are pressed while holding the Shift Key too.")> _
    <DefaultValue(10)> _
    Public Property ChangeLarge() As Integer
        Get
            Return _ChangeLarge
        End Get
        Set(ByVal value As Integer)
            _ChangeLarge = Math.Abs(value)
        End Set
    End Property

    Private _ChangeSmall As Integer = 1
    <Category("Appearance gTrackBar")> _
    <Description("How far to adjust the value when clicking the Arrow buttons or when the Arrow Keys are pressed")> _
    <DefaultValue(1)> _
    Public Property ChangeSmall() As Integer
        Get
            Return _ChangeSmall
        End Get
        Set(ByVal value As Integer)
            _ChangeSmall = Math.Abs(value)
        End Set
    End Property

    Private _BorderShow As Boolean
    <Category("Appearance gTrackBar")> _
    <Description("Show or not show the border around the control")> _
    <DefaultValue(False)> _
    Public Property BorderShow() As Boolean
        Get
            Return _BorderShow
        End Get
        Set(ByVal value As Boolean)
            _BorderShow = value
            Invalidate()
        End Set
    End Property

    Private _ShowFocus As Boolean = True
    <Category("Appearance gTrackBar")> _
    <Description("Show or not show when the control has focus")> _
    <DefaultValue(True)> _
    Public Property ShowFocus() As Boolean
        Get
            Return _ShowFocus
        End Get
        Set(ByVal value As Boolean)
            _ShowFocus = value
        End Set
    End Property

    Private _JumpToMouse As Boolean
    <Category("Behavior Slider")> _
    <Description("Get or Set if the Slider Jumps to the mouse position or increments to it")> _
    <DefaultValue(False)> _
    Public Property JumpToMouse() As Boolean
        Get
            Return _JumpToMouse
        End Get
        Set(ByVal value As Boolean)
            _JumpToMouse = value
        End Set
    End Property

    Private _SnapToValue As Boolean = True
    <Category("Behavior Slider")> _
    <Description("Get or Set if the Slider Jumps to the Value position when the mouse is up")> _
    <DefaultValue(True)> _
    Public Property SnapToValue() As Boolean
        Get
            Return _SnapToValue
        End Get
        Set(ByVal value As Boolean)
            _SnapToValue = value
        End Set
    End Property

#End Region

#Region "Ticks"

    Private _TickType As eTickType = eTickType.None
    <Category("Appearance gTrackBar")> _
    <Description("Where to draw the Tick Marks")> _
    <DefaultValue(GetType(eTickType), "None")> _
    Public Property TickType() As eTickType
        Get
            Return _TickType
        End Get
        Set(ByVal value As eTickType)
            _TickType = value
            Invalidate()
        End Set
    End Property

    Private _TickInterval As Integer = 10
    <Category("Appearance gTrackBar")> _
    <Description("The Interval between the Tick Marks")> _
    <DefaultValue(10)> _
    Public Property TickInterval() As Integer
        Get
            Return _TickInterval
        End Get
        Set(ByVal value As Integer)
            _TickInterval = value
            Invalidate()
        End Set
    End Property

    Private _TickWidth As Integer = 5
    <Category("Appearance gTrackBar")> _
    <Description("How long to draw the Tick Marks")> _
    <DefaultValue(5)> _
    Public Property TickWidth() As Integer
        Get
            Return _TickWidth
        End Get
        Set(ByVal value As Integer)
            _TickWidth = value
            Invalidate()
        End Set
    End Property

    Private _TickThickness As Single = 1
    <Category("Appearance gTrackBar")> _
    <Description("How Thick to draw the Tick Marks")> _
    <DefaultValue(1)> _
    Public Property TickThickness() As Single
        Get
            Return _TickThickness
        End Get
        Set(ByVal value As Single)
            _TickThickness = value
            Invalidate()
        End Set
    End Property

    Private _TickOffset As Integer = 10
    <Category("Appearance gTrackBar")> _
    <Description("How far to offset the Tick Marks")> _
    <DefaultValue(10)> _
    Public Property TickOffset() As Integer
        Get
            Return _TickOffset
        End Get
        Set(ByVal value As Integer)
            _TickOffset = value
            Invalidate()
        End Set
    End Property

#End Region

#Region "FloatValue"

    Private _FloatValue As Boolean = True
    <Category("Appearance FloatValue")> _
    <Description("Show or not show the value above the slider while dragging it back and forth")> _
    <DefaultValue(True)> _
    Public Property FloatValue() As Boolean
        Get
            Return _FloatValue
        End Get
        Set(ByVal value As Boolean)
            _FloatValue = value
        End Set
    End Property

    Private _FloatValueFont As Font = New Font("Arial", 8, FontStyle.Bold)
    <Category("Appearance FloatValue")> _
    <Description("Font to use for the value above the slider ")> _
    <DefaultValue(GetType(Font), "Arial, 8pt, style=Bold")> _
    Public Property FloatValueFont() As Font
        Get
            Return _FloatValueFont
        End Get
        Set(ByVal value As Font)
            _FloatValueFont = value
            Invalidate()
        End Set
    End Property

#End Region

#Region "Label"

    Private _Label As String = "1000"

    <Category("Appearance Label")> _
    <Description("Text to appear as a label on the control")> _
    Public Property Label() As String
        Get
            Return _Label
        End Get
        Set(ByVal value As String)
            _Label = value
            Invalidate()
        End Set
    End Property

    Private _LabelFont As Font = New Font("Arial", 12, FontStyle.Bold)
    <Category("Appearance Label")> _
    <Description("Font to use for the Label Text")> _
    <DefaultValue(GetType(Font), "Arial, 12pt, style=Bold")> _
    Public Property LabelFont() As Font
        Get
            Return _LabelFont
        End Get
        Set(ByVal value As Font)
            _LabelFont = value
            UpdateRects()
            Invalidate()
        End Set
    End Property

    Private ReadOnly _Labelsf As New StringFormat()
    Private _LabelAlighnment As StringAlignment = StringAlignment.Near
    <Category("Appearance Label")> _
    <Description("Alignment for the Label Text")> _
    <DefaultValue(GetType(StringAlignment), "Near")> _
    Public Property LabelAlighnment() As StringAlignment
        Get
            Return _LabelAlighnment
        End Get
        Set(ByVal value As StringAlignment)
            _LabelAlighnment = value
            _Labelsf.Alignment = value
            _Labelsf.Trimming = StringTrimming.EllipsisCharacter
            Invalidate()
        End Set
    End Property

    Private _LabelPadding As Padding = New Padding(3)
    <Category("Appearance Label")> _
    <Description("Pad the Label Text from the edge of the Control")> _
    <DefaultValue(GetType(Padding), "3, 3, 3, 3")> _
    Public Property LabelPadding() As Padding
        Get
            Return _LabelPadding
        End Get
        Set(ByVal value As Padding)
            _LabelPadding = value
            Padding = value
            UpdateRects()
            Invalidate()
        End Set
    End Property

    Private _LabelShow As Boolean
    <Category("Appearance Label")> _
    <Description("Show or not show the Label Text")> _
    <DefaultValue(False)> _
    Public Property LabelShow() As Boolean
        Get
            Return _LabelShow
        End Get
        Set(ByVal value As Boolean)
            _LabelShow = value
            UpdateRects()

            Invalidate()
        End Set
    End Property

#End Region 'Label

#Region "Slider"

    Private _SliderWidthHigh As Single = 1
    <Category("Appearance Slider")> _
    <Description("How wide to make the High side of the Slider Line")> _
    <DefaultValue(1)> _
    Public Property SliderWidthHigh() As Single
        Get
            Return _SliderWidthHigh
        End Get
        Set(ByVal value As Single)
            _SliderWidthHigh = value
            Invalidate()
        End Set
    End Property

    Private _SliderWidthLow As Single = 1
    <Category("Appearance Slider")> _
    <Description("How wide to make Low side of the Slider Line")> _
    <DefaultValue(1)> _
    Public Property SliderWidthLow() As Single
        Get
            Return _SliderWidthLow
        End Get
        Set(ByVal value As Single)
            _SliderWidthLow = value
            Invalidate()
        End Set
    End Property

    Private _SliderImage As Bitmap
    <Category("Appearance Slider")> _
    <Description("Slider Image")> _
    <DefaultValue(GetType(Bitmap), "none")> _
    Public Property SliderImage() As Bitmap
        Get
            Return _SliderImage
        End Get
        Set(ByVal value As Bitmap)
            _SliderImage = value
            Invalidate()
        End Set
    End Property

    Private _SliderCapStart As LineCap = LineCap.Round
    <Category("Appearance Slider")> _
    <Description("Cap style to use for the start of the Slider Line")> _
    <DefaultValue(GetType(LineCap), "Round")> _
    Public Property SliderCapStart() As LineCap
        Get
            Return _SliderCapStart
        End Get
        Set(ByVal value As LineCap)
            _SliderCapStart = value
            Invalidate()
        End Set
    End Property

    Private _SliderCapEnd As LineCap = LineCap.Round
    <Category("Appearance Slider")> _
    <Description("Cap style to use for the end of the Slider Line")> _
    <DefaultValue(GetType(LineCap), "Round")> _
    Public Property SliderCapEnd() As LineCap
        Get
            Return _SliderCapEnd
        End Get
        Set(ByVal value As LineCap)
            _SliderCapEnd = value
            Invalidate()
        End Set
    End Property

    Private _SliderSize As Size = New Size(20, 10)
    <Category("Appearance Slider")> _
    <Description("Size of the Slider")> _
    <DefaultValue(GetType(Size), "20, 10")> _
    Public Property SliderSize() As Size
        Get
            Return _SliderSize
        End Get
        Set(ByVal value As Size)
            _SliderSize = value
            If _Orientation = Windows.Forms.Orientation.Horizontal Then
                intSlideIndent = CInt(value.Width / 2) + 5
            Else
                intSlideIndent = CInt(value.Height / 2) + 5
            End If
            UpdateRects()
            Invalidate()
        End Set
    End Property

    Private _SliderShape As eShape = eShape.Ellipse
    <Category("Appearance Slider")> _
    <Description("Shape for the Slider")> _
    <DefaultValue(GetType(eShape), "Ellipse")> _
    Public Property SliderShape() As eShape
        Get
            Return _SliderShape
        End Get
        Set(ByVal value As eShape)
            _SliderShape = value
            SetSliderPath()
            Invalidate()
        End Set
    End Property

    Private _SliderHighlightPt As PointF = New PointF(-5.0F, -2.5F)
    <Category("Appearance Slider")> _
    <Description("Point on the Slider for the Highlight Color")> _
    <TypeConverter(GetType(PointFConverter))> _
    Public Property SliderHighlightPt() As PointF
        Get
            Return _SliderHighlightPt
        End Get
        Set(ByVal value As PointF)
            _SliderHighlightPt = value
            Invalidate()
        End Set
    End Property

#Region "SliderHighlightPt Default Value"

    Public Sub ResetSliderHighlightPt()
        SliderHighlightPt = New PointF(-5.0F, -2.5F)
    End Sub

    Private Function ShouldSerializeSliderHighlightPt() As Boolean
        Return Not (_SliderHighlightPt.Equals(New PointF(-5.0F, -2.5F)))
    End Function
#End Region

    Private _SliderFocalPt As PointF = New PointF(0.0F, 0.0F)
    <Category("Appearance Slider")> _
    <Description("Focus of the Center Point")> _
    <TypeConverter(GetType(PointFConverter))> _
    Public Property SliderFocalPt() As PointF
        Get
            Return _SliderFocalPt
        End Get
        Set(ByVal value As PointF)
            _SliderFocalPt = value
            Invalidate()
        End Set
    End Property

#Region "SliderFocalPt Default Value"

    Public Sub ResetSliderFocalPt()
        SliderFocalPt = New PointF(0.0F, 0.0F)
    End Sub

    Private Function ShouldSerializeSliderFocalPt() As Boolean
        Return Not (_SliderFocalPt.Equals(New PointF(0.0F, 0.0F)))
    End Function
#End Region

#End Region 'Slider

#Region "ValueBox"

    Private _ValueBox As eValueBox = eValueBox.None
    <Category("Appearance ValueBox")> _
    <Description("Where to draw the Value Box")> _
    <DefaultValue(GetType(eValueBox), "None")> _
    Public Property ValueBox() As eValueBox
        Get
            Return _ValueBox
        End Get
        Set(ByVal value As eValueBox)
            _ValueBox = value
            SetSliderRect()
            Invalidate()
        End Set
    End Property

    Private _ValueBoxSize As Size = New Size(30, 20)
    <Category("Appearance ValueBox")> _
    <Description("What size to draw the Value Box")> _
    <DefaultValue(GetType(Size), "30, 20")> _
    Public Property ValueBoxSize() As Size
        Get
            Return _ValueBoxSize
        End Get
        Set(ByVal value As Size)
            _ValueBoxSize = value
            rectValueBox.Width = value.Width
            rectValueBox.Height = value.Height
            SetSliderRect()
            Invalidate()
        End Set
    End Property

    Private _ValueBoxFont As Font = New Font("Arial", 8.25)
    <Category("Appearance ValueBox")> _
    <Description("What font to use in the Value Box")> _
    <DefaultValue(GetType(Font), "Arial, 8.25pt")> _
    Public Property ValueBoxFont() As Font
        Get
            Return _ValueBoxFont
        End Get
        Set(ByVal value As Font)
            _ValueBoxFont = value
            Invalidate()
        End Set
    End Property

    Private _ValueBoxShape As eShape = eShape.Rectangle
    <Category("Appearance ValueBox")> _
    <Description("What Shape to draw the Value Box")> _
    <DefaultValue(GetType(eShape), "Rectangle")> _
    Public Property ValueBoxShape() As eShape
        Get
            Return _ValueBoxShape
        End Get
        Set(ByVal value As eShape)
            _ValueBoxShape = value
            Invalidate()
        End Set
    End Property

#End Region 'Value Box

#Region "UpDownButtons"

    Private _UpDownWidth As Integer = 30
    <Category("Appearance UpDownButtons")> _
    <Description("Width to draw the Up and Down Buttons if not set to Auto")> _
    <DefaultValue(30)> _
    Public Property UpDownWidth() As Integer
        Get
            Return _UpDownWidth
        End Get
        Set(ByVal value As Integer)
            If value < 10 Then value = 10
            _UpDownWidth = value
            SetUpDnButtonsRect()
            Invalidate()
        End Set
    End Property

    Private _UpDownAutoWidth As Boolean = True
    <Category("Appearance UpDownButtons")> _
    <Description("Auto Size the Buttons to the Control")> _
    <DefaultValue(True)> _
    Public Property UpDownAutoWidth() As Boolean
        Get
            Return _UpDownAutoWidth
        End Get
        Set(ByVal value As Boolean)
            _UpDownAutoWidth = value
            SetUpDnButtonsRect()
            Invalidate()
        End Set
    End Property

    Private _UpDownShow As Boolean = True
    <Category("Appearance UpDownButtons")> _
    <Description("Get or Set if the Up and Down buttons are shown")> _
    <DefaultValue(True)> _
    Public Property UpDownShow() As Boolean
        Get
            Return _UpDownShow
        End Get
        Set(ByVal value As Boolean)
            _UpDownShow = value
            SetSliderRect()
            Invalidate()
        End Set
    End Property

#End Region 'UpDownButtons

#Region "Colors"

    Private _ColorUp As ColorPack = New ColorPack()
    <Category("Appearance Slider")> _
    <Description("Main Color of the Slider when State is Up")> _
    <Editor(GetType(ColorPackEditor), GetType(UITypeEditor))> _
    <TypeConverter(GetType(ColorPackConverter))> _
    Public Property ColorUp() As ColorPack
        Get
            Return _ColorUp
        End Get
        Set(ByVal value As ColorPack)
            _ColorUp = value
            CurrSliderColor = _ColorUp.Face
            CurrSliderBorderColor = _ColorUp.Border
            CurrSliderHiLtColor = _ColorUp.Highlight
            Invalidate()
        End Set
    End Property

#Region "ColorUp Default Value"

    Public Sub ResetColorUp()
        ColorUp = New ColorPack
    End Sub

    Private Function ShouldSerializeColorUp() As Boolean
        Return Not (_ColorUp.Equals(New ColorPack))
    End Function
#End Region

    Private _ColorDown As ColorPack = New ColorPack(Color.CornflowerBlue, Color.DarkSlateBlue, Color.AliceBlue)
    <Category("Appearance Slider")> _
    <Description("Main Color of the Slider when State is Down")> _
    <Editor(GetType(ColorPackEditor), GetType(UITypeEditor))> _
    <TypeConverter(GetType(ColorPackConverter))> _
    Public Property ColorDown() As ColorPack
        Get
            Return _ColorDown
        End Get
        Set(ByVal value As ColorPack)
            _ColorDown = value
            Invalidate()
        End Set
    End Property

#Region "ColorDown Default Value"

    Public Sub ResetColorDown()
        ColorDown = New ColorPack(Color.CornflowerBlue, Color.DarkSlateBlue, Color.AliceBlue)
    End Sub

    Private Function ShouldSerializeColorDown() As Boolean
        Return Not (_ColorDown.Equals(New ColorPack(Color.CornflowerBlue, Color.DarkSlateBlue, Color.AliceBlue)))
    End Function
#End Region

    Private _ColorHover As ColorPack = New ColorPack(Color.Blue, Color.RoyalBlue, Color.White)
    <Category("Appearance Slider")> _
    <Description("Main Color of the Slider when State is Hovering")> _
    <Editor(GetType(ColorPackEditor), GetType(UITypeEditor))> _
    <TypeConverter(GetType(ColorPackConverter))> _
    Public Property ColorHover() As ColorPack
        Get
            Return _ColorHover
        End Get
        Set(ByVal value As ColorPack)
            _ColorHover = value
            Invalidate()
        End Set
    End Property

#Region "ColorHover Default Value"

    Public Sub ResetColorHover()
        ColorHover = New ColorPack(Color.Blue, Color.RoyalBlue, Color.White)
    End Sub

    Private Function ShouldSerializeColorHover() As Boolean
        Return Not (_ColorHover.Equals(New ColorPack(Color.Blue, Color.RoyalBlue, Color.White)))
    End Function
#End Region

    Private _BorderColor As Color = Color.Black
    <Category("Appearance gTrackBar")> _
    <Description("The Color of the Border around the Control")> _
    <DefaultValue(GetType(Color), "Black")> _
    Public Property BorderColor() As Color
        Get
            Return _BorderColor
        End Get
        Set(ByVal value As Color)
            _BorderColor = value
            Invalidate()
        End Set
    End Property

    Private _SliderColorLow As ColorLinearGradient = New ColorLinearGradient(Color.Red, Color.Red)
    <Category("Appearance Slider")> _
    <Description("The Color of the Slider Line on the Low Value Side")> _
    <Editor(GetType(ColorLinearGradientEditor), GetType(UITypeEditor))> _
    <TypeConverter(GetType(ColorLinearGradientConverter))> _
    Public Property SliderColorLow() As ColorLinearGradient
        Get
            Return _SliderColorLow
        End Get
        Set(ByVal value As ColorLinearGradient)
            _SliderColorLow = value
            Invalidate()
        End Set
    End Property

#Region "SliderColorLow Default Value"

    Public Sub ResetSliderColorLow()
        SliderColorLow = New ColorLinearGradient(Color.Red, Color.Red)
    End Sub

    Private Function ShouldSerializeSliderColorLow() As Boolean
        Return Not (_SliderColorLow.Equals(New ColorLinearGradient(Color.Red, Color.Red)))
    End Function
#End Region

    Private _SliderColorHigh As ColorLinearGradient = New ColorLinearGradient(Color.DarkGray, Color.DarkGray)
    <Category("Appearance Slider")> _
    <Description("The Color of the Slider Line on the High Value Side")> _
    <Editor(GetType(ColorLinearGradientEditor), GetType(UITypeEditor))> _
    <TypeConverter(GetType(ColorLinearGradientConverter))> _
    Public Property SliderColorHigh() As ColorLinearGradient
        Get
            Return _SliderColorHigh
        End Get
        Set(ByVal value As ColorLinearGradient)
            _SliderColorHigh = value
            Invalidate()
        End Set
    End Property

#Region "SliderColorHigh Default Value"

    Public Sub ResetSliderColorHigh()
        SliderColorHigh = New ColorLinearGradient(Color.DarkGray, Color.DarkGray)
    End Sub

    Private Function ShouldSerializeSliderColorHigh() As Boolean
        Return Not (_SliderColorHigh.Equals(New ColorLinearGradient(Color.DarkGray, Color.DarkGray)))
    End Function
#End Region

    Private _ArrowColorUp As Color = Color.LightSteelBlue
    <Category("Appearance UpDownButtons")> _
    <Description("Color of the Button Arrow when the State is Up")> _
    <DefaultValue(GetType(Color), "LightSteelBlue")> _
    Public Property ArrowColorUp() As Color
        Get
            Return _ArrowColorUp
        End Get
        Set(ByVal value As Color)
            _ArrowColorUp = value
            Invalidate()
        End Set
    End Property

    Private _ArrowColorDown As Color = Color.GhostWhite
    <Category("Appearance UpDownButtons")> _
    <Description("Color of the Button Arrow when the State is Down")> _
    <DefaultValue(GetType(Color), "GhostWhite")> _
    Public Property ArrowColorDown() As Color
        Get
            Return _ArrowColorDown
        End Get
        Set(ByVal value As Color)
            _ArrowColorDown = value
            Invalidate()
        End Set
    End Property

    Private _ArrowColorHover As Color = Color.DarkBlue
    <Category("Appearance UpDownButtons")> _
    <Description("Color of the Button Arrow when the State is Hovering")> _
    <DefaultValue(GetType(Color), "DarkBlue")> _
    Public Property ArrowColorHover() As Color
        Get
            Return _ArrowColorHover
        End Get
        Set(ByVal value As Color)
            _ArrowColorHover = value
            Invalidate()
        End Set
    End Property

    Private _AButColor As ColorPack = New ColorPack(Color.SteelBlue, Color.CornflowerBlue, Color.Lavender)
    <Category("Appearance UpDownButtons")> _
    <Description("Color of the Up Down Button")> _
    <Editor(GetType(ColorPackEditor), GetType(UITypeEditor))> _
    <TypeConverter(GetType(ColorPackConverter))> _
    Public Property AButColor() As ColorPack
        Get
            Return _AButColor
        End Get
        Set(ByVal value As ColorPack)
            _AButColor = value
            Invalidate()
        End Set
    End Property

#Region "AButColor Default Value"

    Public Sub ResetAButColor()
        AButColor = New ColorPack(Color.SteelBlue, Color.CornflowerBlue, Color.Lavender)
    End Sub

    Private Function ShouldSerializeAButColor() As Boolean
        Return Not (_AButColor.Equals(New ColorPack(Color.SteelBlue, Color.CornflowerBlue, Color.Lavender)))
    End Function
#End Region

    Private _ValueBoxBackColor As Color = Color.White
    <Category("Appearance ValueBox")> _
    <Description("Background Color for the Value Box")> _
    <DefaultValue(GetType(Color), "White")> _
    Public Property ValueBoxBackColor() As Color
        Get
            Return _ValueBoxBackColor
        End Get
        Set(ByVal value As Color)
            _ValueBoxBackColor = value
            Invalidate()
        End Set
    End Property

    Private _ValueBoxBorder As Color = Color.MediumBlue
    <Category("Appearance ValueBox")> _
    <Description("Color of the Border for the Value Box")> _
    <DefaultValue(GetType(Color), "MediumBlue")> _
    Public Property ValueBoxBorder() As Color
        Get
            Return _ValueBoxBorder
        End Get
        Set(ByVal value As Color)
            _ValueBoxBorder = value
            Invalidate()
        End Set
    End Property

    Private _ValueBoxFontColor As Color = Color.MediumBlue
    <Category("Appearance ValueBox")> _
    <Description("Color of the Font for the Value Box")> _
    <DefaultValue(GetType(Color), "MediumBlue")> _
    Public Property ValueBoxFontColor() As Color
        Get
            Return _ValueBoxFontColor
        End Get
        Set(ByVal value As Color)
            _ValueBoxFontColor = value
            Invalidate()
        End Set
    End Property

    Private _LabelColor As Color = Color.MediumBlue
    <Category("Appearance Label")> _
    <Description("Color of the Label Text")> _
    <DefaultValue(GetType(Color), "MediumBlue")> _
    Public Property LabelColor() As Color
        Get
            Return _LabelColor
        End Get
        Set(ByVal value As Color)
            _LabelColor = value
            Invalidate()
        End Set
    End Property

    Private _FloatValueFontColor As Color = Color.MediumBlue
    <Category("Appearance FloatValue")> _
    <Description("Color of the Value doubleing above the Slider")> _
    <DefaultValue(GetType(Color), "MediumBlue")> _
    Public Property FloatValueFontColor() As Color
        Get
            Return _FloatValueFontColor
        End Get
        Set(ByVal value As Color)
            _FloatValueFontColor = value
            Invalidate()
        End Set
    End Property

    Private _TickColor As Color = Color.DarkGray
    <Category("Appearance Slider")> _
    <Description("Color of the Tick Marks")> _
    <DefaultValue(GetType(Color), "DarkGray")> _
    Public Property TickColor() As Color
        Get
            Return _TickColor
        End Get
        Set(ByVal value As Color)
            _TickColor = value
            Invalidate()
        End Set
    End Property

#End Region 'Colors

#End Region

#Region "Painting"

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)

        'Setup the Graphics
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        'Draw a Border around the control if requested
        If _BorderShow Then
            g.DrawRectangle(New Pen(_BorderColor), _
                            0, 0, Width - 1, Height - 1)
        End If

        'Add the value increment buttons if requested
        If _UpDownShow Then DrawUpDnButtons(g)

        'Add the Line and Tick Marks
        DrawSliderLine(g)

        'Draw the Label Text if requested
        If _LabelShow Then
            DrawLabel(g)
            'g.DrawRectangle(Pens.Gray, rectLabel)
        End If

        'Add the Slider button
        DrawSlider(g)

        'Draw the Value above the Slider if requested
        If _FloatValue AndAlso IsOverSlider AndAlso _
        MouseState = eMouseState.Down Then
            DrawFloatValue(g)
        End If

        'Draw the Box displating the value if requested
        If Not _ValueBox = eValueBox.None Then
            DrawValueBox(g)
        End If

        'Draw Focus Rectangle around control if requested 
        If _ShowFocus AndAlso Focused Then
            ControlPaint.DrawFocusRectangle(g, New Rectangle( _
                                            2 + CInt(Not _BorderShow), 2 + CInt(Not _BorderShow), _
                                            Width - ((2 + CInt(Not _BorderShow)) * 2), _
                                            Height - ((2 + CInt(Not _BorderShow)) * 2)), _
                                            Color.Black, BackColor)
        End If


    End Sub

    Private Sub DrawLabel(ByRef g As Graphics)
        If _Orientation = Windows.Forms.Orientation.Horizontal Then
            _Labelsf.FormatFlags = Nothing
        Else
            _Labelsf.FormatFlags = StringFormatFlags.DirectionVertical
        End If
        g.DrawString(_Label, _LabelFont, New SolidBrush(_LabelColor), rectLabel, _Labelsf)
    End Sub

    Private Sub DrawSlider(ByRef g As Graphics)
        Select Case _BrushStyle
            Case eBrushStyle.Linear

                Using br As LinearGradientBrush = New LinearGradientBrush(gpSlider.GetBounds, _
                                                                          CurrSliderHiLtColor, CurrSliderColor, _BrushDirection)

                    g.FillPath(br, gpSlider)

                End Using

            Case eBrushStyle.Linear2
                Dim blend As ColorBlend = New ColorBlend()
                Dim bColors As Color() = New Color() { _
                    CurrSliderColor, _
                    CurrSliderColor, _
                    CurrSliderHiLtColor, _
                    CurrSliderColor, _
                    CurrSliderColor}
                blend.Colors = bColors

                Dim bPts As Single() = New Single() { _
                    0, _
                    _SliderFocalPt.X, _
                    0.5, _
                    _SliderFocalPt.Y, _
                    1}
                blend.Positions = bPts

                Using br As LinearGradientBrush = New LinearGradientBrush(gpSlider.GetBounds, _
                                                                          CurrSliderColor, CurrSliderHiLtColor, _BrushDirection)
                    br.InterpolationColors = blend
                    g.FillPath(br, gpSlider)

                End Using

            Case eBrushStyle.Path

                Using br As PathGradientBrush = New PathGradientBrush(gpSlider)
                    br.SurroundColors = New Color() {CurrSliderColor}
                    br.CenterColor = CurrSliderHiLtColor
                    br.CenterPoint = New PointF(br.CenterPoint.X + SliderHighlightPt.X, _
                                                br.CenterPoint.Y + SliderHighlightPt.Y)
                    br.FocusScales = _SliderFocalPt
                    g.FillPath(br, gpSlider)
                End Using

            Case eBrushStyle.Image


        End Select

        If _BrushStyle = eBrushStyle.Image Then
            If _SliderImage Is Nothing Then
                g.DrawRectangle(New Pen(CurrSliderBorderColor), Rectangle.Round(gpSlider.GetBounds))
            Else
                g.DrawImage(_SliderImage, gpSlider.GetBounds)
            End If
        Else
            g.DrawPath(New Pen(CurrSliderBorderColor), gpSlider)
        End If

    End Sub

    Private Sub DrawFloatValue(ByRef g As Graphics)
        Dim sz As SizeF = g.MeasureString(Format(ValueAdjusted, _valueStrFormat), _FloatValueFont, _
                                          New PointF(0, 0), StringFormat.GenericDefault)
        Dim rect As Rectangle
        Dim pbr As PathGradientBrush
        Dim gp As New GraphicsPath
        If _Orientation = Windows.Forms.Orientation.Horizontal Then
            rect = New Rectangle(CInt(sngSliderPos - (sz.Width / 2)), _
                                 CInt((rectSlider.Height / 2) + rectSlider.Y - _
                                 (_SliderSize.Height / 2) - 1 - sz.Height), _
                                 CInt(sz.Width) + 1, CInt(sz.Height))
        Else
            rect = New Rectangle(CInt((rectSlider.Width / 2) - (sz.Width / 2)), _
                                 CInt(sngSliderPos - sz.Height - (_SliderSize.Height / 2) - 3), _
                                 CInt(sz.Width + 1), CInt(sz.Height + 2))
        End If
        gp.AddRectangle(rect)
        pbr = New PathGradientBrush(gp)
        pbr.SurroundColors = New Color() {Color.Transparent}
        If BackColor = Color.Transparent Then
            pbr.CenterColor = Parent.BackColor
        Else
            pbr.CenterColor = BackColor
        End If
        g.FillRectangle(pbr, rect)
        rect.Y += 2
        g.DrawString(Format(ValueAdjusted, _valueStrFormat), _FloatValueFont, New SolidBrush(_FloatValueFontColor), rect, sf)
        pbr.Dispose()
        gp.Dispose()
    End Sub

    Private Sub DrawValueBox(ByRef g As Graphics)

        Using bbr As Brush = New SolidBrush(_ValueBoxBackColor), _
        pn As Pen = New Pen(_ValueBoxBorder), _
        fbr As Brush = New SolidBrush(_ValueBoxFontColor)
            Dim rect As Rectangle = New Rectangle( _
            rectValueBox.X, rectValueBox.Y, rectValueBox.Width, rectValueBox.Height)
            If ValueBoxShape = eShape.Rectangle Then
                g.FillRectangle(bbr, rect)
                g.DrawRectangle(pn, rect.X, rect.Y, rect.Width, rect.Height)
            Else
                g.FillEllipse(bbr, rect)
                g.DrawEllipse(pn, rect.X, rect.Y, rect.Width, rect.Height)
            End If

            g.DrawString(Format(ValueAdjusted, _valueStrFormat), _ValueBoxFont, fbr, New Rectangle( _
                         rect.X, rect.Y + 1, rect.Width + 1, rect.Height + 1), sf)
        End Using

    End Sub

    Private Sub DrawUpDnButtons(ByRef g As Graphics)
        Using pn As Pen = New Pen(_ArrowColorUp, 2)
            pn.EndCap = LineCap.Round
            pn.StartCap = LineCap.Round
            pn.LineJoin = LineJoin.Round
            Dim gp As New GraphicsPath
            Dim pts() As Point
            Dim mx As New Matrix
            pts = New Point() { _
                New Point(5, 0), _
                New Point(0, 5), _
                New Point(5, 10)}
            gp.AddLines(pts)

            If _Orientation = Windows.Forms.Orientation.Horizontal Then

                If IsOverDownButton Then
                    g.FillRectangle(New LinearGradientBrush(rectDownButton, _
                                                            _AButColor.Highlight, _AButColor.Face, LinearGradientMode.Horizontal), rectDownButton)
                    If MouseState = eMouseState.Down Then
                        pn.Color = _ArrowColorDown
                    Else
                        pn.Color = _ArrowColorHover
                    End If
                    g.DrawRectangle(New Pen(_AButColor.Border), New Rectangle( _
                    rectDownButton.X + 1, rectDownButton.Y, _
                    rectDownButton.Width - 1, rectDownButton.Height - 1))
                End If
                With rectDownButton
                    mx.Translate(5, CSng((rectDownButton.Y _
                    + (rectDownButton.Height / 2)) - 6))
                    gp.Transform(mx)
                    g.DrawPath(pn, gp)
                End With

                pn.Color = _ArrowColorUp
                If IsOverUpButton Then
                    g.FillRectangle(New LinearGradientBrush(rectUpButton, _
                                                            _AButColor.Face, _AButColor.Highlight, LinearGradientMode.Horizontal), rectUpButton)
                    If MouseState = eMouseState.Down Then
                        pn.Color = _ArrowColorDown
                    Else
                        pn.Color = _ArrowColorHover
                    End If
                    g.DrawRectangle(New Pen(_AButColor.Border), New Rectangle( _
                    rectUpButton.X, rectUpButton.Y, _
                    rectUpButton.Width - 1, rectUpButton.Height - 1))
                End If
                With rectUpButton
                    mx = New Matrix(-1, 0, 0, 1, 5, 0)
                    mx.Translate(.X + 9, 0, MatrixOrder.Append)
                    gp.Transform(mx)
                    g.DrawPath(pn, gp)
                End With
            Else

                If IsOverUpButton Then
                    g.FillRectangle(New LinearGradientBrush(rectUpButton, _
                                                            _AButColor.Face, _AButColor.Highlight, LinearGradientMode.Vertical), rectUpButton)
                    g.DrawRectangle(New Pen(_AButColor.Border), New Rectangle( _
                    rectUpButton.X, rectUpButton.Y, rectUpButton.Width - 1, rectUpButton.Height - 1))
                    If MouseState = eMouseState.Down Then
                        pn.Color = _ArrowColorDown
                    Else
                        pn.Color = _ArrowColorHover
                    End If
                End If
                With rectUpButton
                    mx.RotateAt(90, New PointF(gp.GetBounds.Width / 2, gp.GetBounds.Height / 2))
                    mx.Translate(CSng((rectDownButton.X + (rectDownButton.Width / 2)) - 3), 4, MatrixOrder.Append)
                    gp.Transform(mx)
                    g.DrawPath(pn, gp)
                End With

                pn.Color = _ArrowColorUp
                If IsOverDownButton Then
                    g.FillRectangle(New LinearGradientBrush(rectDownButton, _
                                                            _AButColor.Highlight, _AButColor.Face, LinearGradientMode.Vertical), rectDownButton)
                    g.DrawRectangle(New Pen(_AButColor.Border), New Rectangle( _
                    rectDownButton.X, rectDownButton.Y, rectDownButton.Width - 1, rectDownButton.Height - 1))
                    If MouseState = eMouseState.Down Then
                        pn.Color = _ArrowColorDown
                    Else
                        pn.Color = _ArrowColorHover
                    End If
                End If
                With rectDownButton
                    mx = New Matrix(1, 0, 0, -1, 0, 10)
                    mx.Translate(0, .Y + 6, MatrixOrder.Append)
                    gp.Transform(mx)
                    g.DrawPath(pn, gp)
                End With
            End If
            mx.Dispose()
            gp.Dispose()
        End Using

    End Sub

    Private Sub DrawSliderLine(ByRef g As Graphics)
        Using pn As Pen = New Pen(_SliderColorLow.ColorA, _SliderWidthLow), _
        tpn As Pen = New Pen(_TickColor, _TickThickness)
            Dim switch As Integer = CInt(IIf(_Orientation = Windows.Forms.Orientation.Horizontal, 1, -1))
            Dim t1, t2 As Single
            Dim lAdj As Integer = 0

            Select Case _TickType
                Case eTickType.Middle
                    t1 = CSng(-_TickWidth / 2)
                    t2 = CSng(_TickWidth / 2)

                Case eTickType.Up_Right
                    t1 = (-5 - _TickOffset - _TickWidth) * switch
                    t2 = (-5 - _TickOffset) * switch

                Case eTickType.Down_Left, eTickType.Both
                    t1 = (5 + _TickOffset + _TickWidth) * switch
                    t2 = (5 + _TickOffset) * switch

            End Select

            If _LabelShow Then
                lAdj += rectLabel.Height + _LabelPadding.Vertical - 4
            End If

            Dim Tickpos As Integer
            If Orientation = Windows.Forms.Orientation.Horizontal Then

                If _TickType <> eTickType.None Then
                    For i As Integer = 0 To _MaxValue - _MinValue Step _TickInterval
                        Tickpos = CInt(rectSlider.X + (rectSlider.Width * _
                        (i / (_MaxValue - _MinValue))))
                        g.DrawLine(tpn, Tickpos, CSng(rectSlider.Height / 2) + t1 + lAdj, _
                                   Tickpos, CSng(rectSlider.Height / 2) + t2 + lAdj)
                        If _TickType = eTickType.Both Then
                            g.DrawLine(tpn, Tickpos, CSng(rectSlider.Height / 2) - t1 + lAdj, _
                                       Tickpos, CSng(rectSlider.Height / 2) - t2 + lAdj)
                        End If
                    Next
                End If

                pn.StartCap = _SliderCapStart
                If _Value = _MaxValue Then
                    pn.EndCap = _SliderCapEnd
                Else
                    pn.EndCap = LineCap.Flat
                End If
                pn.Brush = New LinearGradientBrush( _
                New PointF(CSng(rectSlider.X - _SliderWidthLow), _
                           CSng(rectSlider.Height / 2 + rectSlider.Y)), _
                New PointF(sngSliderPos + _SliderWidthLow, CSng(rectSlider.Height / 2 + rectSlider.Y)), _
                _SliderColorLow.ColorA, _SliderColorLow.ColorB)
                g.DrawLine(pn, CSng(rectSlider.X), CSng(rectSlider.Height / 2 + rectSlider.Y), _
                           sngSliderPos + 1, CSng(rectSlider.Height / 2 + rectSlider.Y))

                If _Value = _MinValue Then
                    pn.StartCap = _SliderCapStart
                Else
                    pn.StartCap = LineCap.Flat
                End If
                pn.EndCap = _SliderCapEnd
                pn.Brush = New LinearGradientBrush( _
                New PointF(sngSliderPos - _SliderWidthHigh, _
                           CSng(rectSlider.Height / 2 + rectSlider.Y)), _
                New PointF(CSng(rectSlider.X + rectSlider.Width + _SliderWidthHigh + 1), _
                           CSng(rectSlider.Height / 2 + rectSlider.Y)), _
                _SliderColorHigh.ColorA, _SliderColorHigh.ColorB)
                pn.Width = _SliderWidthHigh
                g.DrawLine(pn, sngSliderPos, CSng(rectSlider.Height / 2 + rectSlider.Y), _
                       CSng(rectSlider.X + rectSlider.Width), CSng(rectSlider.Height / 2 + rectSlider.Y))
            Else

                If _TickType <> eTickType.None Then
                    For i As Integer = 0 To _MaxValue Step _TickInterval
                        Tickpos = CInt(rectSlider.Y + (rectSlider.Height * _
                        ((i - _MinValue) / (_MaxValue - _MinValue))))
                        g.DrawLine(tpn, CSng(rectSlider.Width / 2) + t1, Tickpos, _
                                   CSng(rectSlider.Width / 2) + t2, Tickpos)
                        If _TickType = eTickType.Both Then
                            g.DrawLine(tpn, CSng(rectSlider.Width / 2) - t1, Tickpos _
                                       , CSng(rectSlider.Width / 2) - t2, Tickpos)
                        End If
                    Next
                End If

                'Bottom
                pn.StartCap = _SliderCapStart
                If _Value = _MaxValue Then
                    pn.EndCap = _SliderCapEnd
                Else
                    pn.EndCap = LineCap.Flat
                End If
                pn.Brush = New LinearGradientBrush( _
                New PointF(CSng(rectSlider.Width / 2), sngSliderPos - _SliderWidthLow), _
                New PointF(CSng(rectSlider.Width / 2), _
                           CSng(rectSlider.Y + rectSlider.Height + _SliderWidthLow + 1)), _
                _SliderColorLow.ColorA, _SliderColorLow.ColorB)

                pn.Width = _SliderWidthLow
                g.DrawLine(pn, CSng(rectSlider.Width / 2), CSng(rectSlider.Y + rectSlider.Height), _
                           CSng(rectSlider.Width / 2), sngSliderPos)

                'top
                If _Value = _MinValue Then
                    pn.StartCap = _SliderCapStart
                Else
                    pn.StartCap = LineCap.Flat
                End If
                pn.EndCap = _SliderCapEnd
                pn.Color = _SliderColorHigh.ColorA
                pn.Width = _SliderWidthHigh
                pn.Brush = New LinearGradientBrush( _
                New PointF(CSng(rectSlider.Width / 2), _
                           CSng(rectSlider.Y - _SliderWidthHigh - 1)), _
                New PointF(CSng(rectSlider.Width / 2), sngSliderPos + _SliderWidthHigh), _
                _SliderColorHigh.ColorA, _SliderColorHigh.ColorB)

                pn.Width = _SliderWidthHigh

                g.DrawLine(pn, CSng(rectSlider.Width / 2), sngSliderPos, _
                           CSng(rectSlider.Width / 2), CSng(rectSlider.Y))
            End If
        End Using

    End Sub

#End Region

#Region "Building"

    Private Sub SetSliderPath()
        gpSlider.Reset()
        Dim rect As RectangleF
        If _Orientation = Windows.Forms.Orientation.Horizontal Then
            rect = New RectangleF(CSng(sngSliderPos - (_SliderSize.Width / 2)), _
                                  CSng(rectSlider.Y + (rectSlider.Height / 2) - (_SliderSize.Height) / 2), _
                                  _SliderSize.Width, _SliderSize.Height)
        Else
            rect = New RectangleF(CSng((rectSlider.Width - _SliderSize.Width) / 2), _
                                  CSng(sngSliderPos - (_SliderSize.Height / 2)) _
                                  , _SliderSize.Width, _SliderSize.Height)
        End If

        Select Case _SliderShape

            Case eShape.Rectangle
                gpSlider.AddRectangle(rect)

            Case eShape.Ellipse
                gpSlider.AddEllipse(rect)

            Case eShape.ArrowUp
                gpSlider.AddPolygon(New PointF() {
                    New PointF(rect.X, rect.Bottom),
                    New PointF(rect.Right, rect.Bottom),
                    New PointF(rect.X + (rect.Width / 2), rect.Top)
                })

            Case eShape.ArrowDown
                gpSlider.AddPolygon(New PointF() {
                    New PointF(rect.X, rect.Top),
                    New PointF(rect.Right, rect.Top),
                    New PointF(rect.X + (rect.Width / 2), rect.Bottom)
                })

            Case eShape.ArrowRight
                gpSlider.AddPolygon(New PointF() {
                    New PointF(rect.X, rect.Bottom),
                    New PointF(rect.Right, rect.Top + (rect.Height / 2)),
                    New PointF(rect.X, rect.Top)
                })

            Case eShape.ArrowLeft
                gpSlider.AddPolygon(New PointF() {
                    New PointF(rect.Right, rect.Bottom),
                    New PointF(rect.X, rect.Top + (rect.Height / 2)),
                    New PointF(rect.Right, rect.Top)
                })

        End Select

        InvRect = Rectangle.Round(gpSlider.GetBounds)
        InvRect.Inflate(2, 2)
    End Sub

    Private Sub UpdateSlider(ByVal xPos As Integer)
        Dim rect As RectangleF = gpSlider.GetBounds
        rect.Inflate(20, 20)
        rect.Offset(-10, -10)
        Invalidate(Rectangle.Round(rect))
        sngSliderPos = xPos
        If _Orientation = Windows.Forms.Orientation.Horizontal Then
            If sngSliderPos - rectSlider.X < 0 Then sngSliderPos = rectSlider.X
            If sngSliderPos > rectSlider.X + rectSlider.Width Then sngSliderPos = rectSlider.X + rectSlider.Width
        Else
            If sngSliderPos - rectSlider.Y < 0 Then sngSliderPos = rectSlider.Y
            If sngSliderPos > rectSlider.Y + rectSlider.Height Then sngSliderPos = rectSlider.Y + rectSlider.Height
        End If
        SetSliderPath()
        Invalidate(Rectangle.Round(rect))
    End Sub

    Private Sub SetUpDnButtonsRect()
        Dim UDWidth, UDY As Integer

        If Orientation = Windows.Forms.Orientation.Horizontal Then
            If _UpDownAutoWidth Then
                UDWidth = rectSlider.Height - 4
                UDY = 3
            Else
                UDWidth = _UpDownWidth
                UDY = CInt((rectSlider.Height - UDWidth) / 2)
            End If

            If _LabelShow Then UDY += rectLabel.Height + _LabelPadding.Vertical - 4

            rectDownButton = New Rectangle(1, UDY, 15, UDWidth)
            rectUpButton = New Rectangle(Width - 17, UDY, 15, UDWidth)
        Else
            If _UpDownAutoWidth Then
                UDWidth = rectSlider.Width - 4
                UDY = 2
            Else
                UDWidth = _UpDownWidth
                UDY = CInt((rectSlider.Width - UDWidth) / 2)
            End If

            rectUpButton = New Rectangle(UDY, 2, UDWidth, 15)
            rectDownButton = New Rectangle(UDY, Height - 17, UDWidth, 15)
        End If
    End Sub

    Private Sub SetLabelRect()
        If Orientation = Windows.Forms.Orientation.Horizontal Then
            rectLabel = New Rectangle(_LabelPadding.Left, _LabelPadding.Top, _
                                      Width - _LabelPadding.Horizontal - 1, LabelFont.Height)
        Else
            rectLabel = New Rectangle(Width - LabelFont.Height - _LabelPadding.Top, _LabelPadding.Left, _
                                      LabelFont.Height, Height - _LabelPadding.Horizontal - 1)
        End If
    End Sub

    Private Sub SetSliderRect()
        Try
            Dim ButtonOffset As Integer = 17
            If Not _UpDownShow Then ButtonOffset = 0
            With rectSlider
                If Orientation = Windows.Forms.Orientation.Horizontal Then
                    Dim _SliderWidth As Single = Math.Max(_SliderWidthLow, _SliderWidthHigh)

                    If _LabelShow Then
                        .Height = Height - rectLabel.Height - _LabelPadding.Top
                    Else
                        .Height = Height - 1
                    End If

                    Select Case _ValueBox
                        Case eValueBox.None
                            .X = ButtonOffset + intSlideIndent
                            .Width = Width - ((ButtonOffset * 2) + 1) - (intSlideIndent * 2)
                        Case eValueBox.Left
                            rectValueBox.X = ButtonOffset + 1
                            rectValueBox.Y = CInt(((rectSlider.Height - rectValueBox.Height) / 2))
                            .Width = CInt(Width - ((ButtonOffset * 2) + 1) - rectValueBox.Width - _
                            (intSlideIndent * 2) - (_SliderWidth / 2))
                            .X = CInt(rectValueBox.Width + ButtonOffset + intSlideIndent + (_SliderWidth / 2))
                        Case eValueBox.Right
                            rectValueBox.X = Width - ButtonOffset - 2 - rectValueBox.Width
                            rectValueBox.Y = CInt(((rectSlider.Height - rectValueBox.Height) / 2))
                            .Width = CInt(Width - ((ButtonOffset * 2) + 1) - rectValueBox.Width - _
                            (intSlideIndent * 2) - (_SliderWidth / 2))
                            .X = ButtonOffset + intSlideIndent
                    End Select

                    If _LabelShow Then
                        .Y = rectLabel.Height + _LabelPadding.Vertical - 4
                        rectValueBox.Y += rectLabel.Height + _LabelPadding.Vertical - 4
                    Else
                        .Y = 0
                    End If
                    UpdateSlider(CInt(rectSlider.X + (rectSlider.Width * _
                    ((_Value - _MinValue) / (_MaxValue - _MinValue)))))

                Else
                    Select Case _ValueBox
                        Case eValueBox.None
                            .Y = ButtonOffset + intSlideIndent
                            .Height = Height - ((ButtonOffset * 2) + 1) - (intSlideIndent * 2)
                        Case eValueBox.Left
                            rectValueBox.X = CInt(((rectSlider.Width - rectValueBox.Width) / 2))
                            rectValueBox.Y = ButtonOffset + 1
                            .Height = CInt(Height - ((ButtonOffset * 2) + 1) - rectValueBox.Height - (intSlideIndent * 2))
                            .Y = CInt(rectValueBox.Height + ButtonOffset + intSlideIndent)
                        Case eValueBox.Right
                            rectValueBox.X = CInt(((rectSlider.Width - rectValueBox.Width) / 2))
                            rectValueBox.Y = Height - ButtonOffset - 2 - rectValueBox.Height
                            .Height = CInt(Height - ((ButtonOffset * 2) + 1) - rectValueBox.Height - (intSlideIndent * 2))
                            .Y = ButtonOffset + intSlideIndent
                    End Select
                    If _LabelShow Then
                        .X = 0
                        .Width = Width - rectLabel.Width - _LabelPadding.Top
                    Else
                        .X = 0
                        .Width = Width - 1
                    End If
                    Dim adj As Integer = 0
                    If _MinValue < 0 Then adj = Math.Abs(_MinValue)
                    UpdateSlider(CInt(rectSlider.Y + (rectSlider.Height * _
                    (((_MaxValue + adj) - _Value - adj) _
                    / ((_MaxValue + adj) - (_MinValue + adj))))))

                End If
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UpdateRects()
        SetLabelRect()
        SetSliderRect()
        SetSliderPath()
        SetUpDnButtonsRect()
    End Sub

#End Region

#Region "Mouse"

    Private InvRect As Rectangle
    Private CurrSliderColor As Color = _ColorUp.Face
    Private CurrSliderBorderColor As Color = _ColorUp.Border
    Private CurrSliderHiLtColor As Color = _ColorUp.Highlight
    '   Private Orient As Integer = 1
    Private MouseHoldDownTicker As Integer
    Private MouseHoldDownChange As Integer

    Private Sub TBSlider_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDown
        MouseState = eMouseState.Down
        MouseHoldDownTicker = 0
        MouseTimer.Interval = 100
        If _UpDownShow Then
            If IsOverDownButton Then
                MouseHoldDownChange = -_ChangeSmall
                Value += MouseHoldDownChange
                MouseTimer.Start()
            ElseIf IsOverUpButton Then
                MouseHoldDownChange = _ChangeSmall
                Value += MouseHoldDownChange
                MouseTimer.Start()
            End If
        End If
        IsOverSlider = gpSlider.IsVisible(e.X, e.Y)
        Dim pos As Integer
        If _Orientation = Windows.Forms.Orientation.Horizontal Then
            pos = e.X
        Else
            pos = e.Y
        End If
        If IsOverSlider Then
            CurrSliderColor = _ColorDown.Face
            CurrSliderBorderColor = _ColorDown.Border
            CurrSliderHiLtColor = _ColorDown.Highlight
        ElseIf rectSlider.Contains(e.Location) Then
            If _JumpToMouse Then
                sngSliderPos = pos
                IsOverSlider = True
                SetSliderValue(New Point(e.X, e.Y))
            Else
                If pos < sngSliderPos Then
                    MouseHoldDownChange = _ChangeLarge * _
                    CInt(IIf(Orientation = Windows.Forms.Orientation.Horizontal, -1, 1))
                    Value += MouseHoldDownChange
                Else
                    MouseHoldDownChange = -(_ChangeLarge * _
                    CInt(IIf(Orientation = Windows.Forms.Orientation.Horizontal, -1, 1)))
                    Value += MouseHoldDownChange
                End If
                MouseTimer.Start()
            End If
        End If
        Invalidate()
    End Sub

    Private Sub gTrackBar_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles Me.MouseLeave
        IsOverDownButton = False
        IsOverUpButton = False
        CurrSliderColor = _ColorUp.Face
        CurrSliderBorderColor = _ColorUp.Border
        CurrSliderHiLtColor = _ColorUp.Highlight
        Invalidate()
    End Sub

    Private Sub TBSlider_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseMove
        If Not IsOverSlider Then
            IsOverDownButton = rectDownButton.Contains(e.Location)
            IsOverUpButton = rectUpButton.Contains(e.Location)
        End If
        Dim rect As Rectangle = rectDownButton
        rect.Inflate(1, 1)
        Invalidate(rect)
        rect = rectUpButton
        rect.Inflate(1, 1)
        Invalidate(rect)

        If MouseState = eMouseState.Up Then IsOverSlider = gpSlider.IsVisible(e.X, e.Y)

        If IsOverSlider And MouseState = eMouseState.Down Then
            SetSliderValue(New Point(e.X, e.Y))

        ElseIf IsOverSlider And MouseState = eMouseState.Up Then
            CurrSliderColor = _ColorHover.Face
            CurrSliderBorderColor = _ColorHover.Border
            CurrSliderHiLtColor = _ColorHover.Highlight
            Invalidate(InvRect)
        Else
            CurrSliderColor = _ColorUp.Face
            CurrSliderBorderColor = _ColorUp.Border
            CurrSliderHiLtColor = _ColorUp.Highlight
            Invalidate(InvRect)
        End If
        Update()
    End Sub

    Private Sub SetSliderValue(ByVal pt As Point)
        If _Orientation = Windows.Forms.Orientation.Horizontal Then
            Value = CInt(((sngSliderPos - rectSlider.X) _
            / (rectSlider.Width / (_MaxValue - _MinValue))) + _MinValue)
            UpdateSlider(pt.X)
        Else
            Dim adj As Integer = 0
            If _MinValue < 0 Then adj = Math.Abs(_MinValue)
            Value = ((_MaxValue + adj) - CInt(((sngSliderPos - rectSlider.Y) _
            / (rectSlider.Height / ((_MaxValue + adj) - (_MinValue + adj)))))) - adj
            UpdateSlider(pt.Y)
        End If

    End Sub

    Private Sub TBSlider_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseUp
        MouseTimer.Stop()
        MouseState = eMouseState.Up
        IsOverDownButton = rectDownButton.Contains(e.Location)
        IsOverUpButton = rectUpButton.Contains(e.Location)
        If _SnapToValue Then SetSliderRect()
        Invalidate()
    End Sub

#End Region

#Region "KeyDown"

    Protected Overrides Function IsInputKey( _
    ByVal keyData As Keys) As Boolean
        'Because a Usercontrol ignores the arrows in the KeyDown Event
        'and changes focus no matter what in the KeyUp Event
        'This is needed to fix the KeyDown problem
        Select Case keyData And Keys.KeyCode
            Case Keys.Up, Keys.Down, Keys.Right, Keys.Left
                Return True
            Case Else
                Return MyBase.IsInputKey(keyData)
        End Select
    End Function

    Private Sub gTrackBar_KeyUp(ByVal sender As Object, _
    ByVal e As KeyEventArgs) Handles Me.KeyUp

        Dim adjust As Integer
        If e.Shift Then
            adjust = _ChangeLarge
        Else
            adjust = _ChangeSmall
        End If

        Select Case e.KeyValue
            Case Keys.Up, Keys.Right
                Value += adjust

            Case Keys.Down, Keys.Left
                Value -= adjust
        End Select
    End Sub

#End Region

#Region "Resize"

    Private Sub TBSlider_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        UpdateRects()
        Refresh()
    End Sub

#End Region

#Region "Focus"

    Private Sub gTrackBar_LostFocus(ByVal sender As Object, ByVal e As EventArgs) _
    Handles Me.LostFocus, Me.GotFocus
        Invalidate()
    End Sub

#End Region

#Region "Mouse Hold Down Timer"

    Private Sub MouseTimer_Tick(ByVal sender As Object, _
    ByVal e As EventArgs) Handles MouseTimer.Tick
        'Check if mouse was just clicked
        If MouseHoldDownTicker < 5 Then
            MouseHoldDownTicker += 1
            'Interval was set to 100 on MouseDown
            'Tick off 5 times and then reset the Timer Interval
            '  based on the Min/Max span
            If MouseHoldDownTicker = 5 Then
                MouseTimer.Interval = CInt(Math.Max _
                (10, 100 - ((_MaxValue - _MinValue) / 10)))
            End If
        Else
            'Change the value until the mouse is released
            Value += MouseHoldDownChange
        End If
    End Sub

#End Region

End Class


