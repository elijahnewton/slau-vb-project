Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Reflection

Public Module UiIcons
    Private ReadOnly Cache As New Dictionary(Of String, Image)()
    Private _appIcon As Icon

    Public Function GetIcon(name As String, size As Integer) As Image
        Dim key = name & ":" & size.ToString()
        If Cache.ContainsKey(key) Then Return Cache(key)

        Using iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Icons." & name & ".png")
            If iconStream Is Nothing Then Return Nothing

            Using source = Image.FromStream(iconStream)
                Dim bitmap As New Bitmap(size, size, PixelFormat.Format32bppArgb)
                Using g = Graphics.FromImage(bitmap)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality
                    g.SmoothingMode = SmoothingMode.HighQuality
                    g.Clear(Color.Transparent)
                    g.DrawImage(source, 0, 0, size, size)
                End Using

                Cache(key) = bitmap
                Return bitmap
            End Using
        End Using
    End Function

    Public Function AppIcon() As Icon
        If _appIcon Is Nothing Then
            Dim image = GetIcon("app", 64)
            If image IsNot Nothing Then
                Using bitmap As New Bitmap(image)
                    _appIcon = Icon.FromHandle(bitmap.GetHicon())
                End Using
            End If
        End If

        Return _appIcon
    End Function

    Public Sub StyleButton(button As Button, iconName As String, Optional size As Integer = 16)
        Dim image = GetIcon(iconName, size)
        If image Is Nothing Then Return

        button.Image = image
        button.ImageAlign = ContentAlignment.MiddleLeft
        button.TextImageRelation = TextImageRelation.ImageBeforeText
        button.Padding = New Padding(3, 0, 3, 0)
    End Sub

    Public Sub StyleTabs(tabs As TabControl, imageList As ImageList, ParamArray iconNames As String())
        imageList.ImageSize = New Size(20, 20)
        imageList.ColorDepth = ColorDepth.Depth32Bit

        For Each name As String In iconNames
            imageList.Images.Add(name, GetIcon(name, 20))
        Next

        tabs.ImageList = imageList
        For index As Integer = 0 To Math.Min(tabs.TabPages.Count, iconNames.Length) - 1
            tabs.TabPages(index).ImageIndex = index
        Next
    End Sub
End Module
