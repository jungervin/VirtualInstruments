   M S B A M L   `   `   `  ????A  =FSComm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:FSCommFSComm  5   	       FSComm.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:FSComm     5   	   -   5          ? Title$  VirtualInstruments??5   	   $	??600??6$   $	??900??61   .??5      ? .+?5   
   o? -   5         o?Name$
 menu??5      $
??Auto??6   $	??Top=?6+   .@?5      m? $??_File??6   .@?5      m? $??Open...??6    m? Command 6,   ?? *OpenCommand+6N   m? 5      $
??Save??6    ? DataContext 6)   ?? *InstrumentsProfileViewModel+ 6]   ?? *SaveProfileCommand+6?   m? 5      $??
Save as...??6    6/   ?? *InstrumentsProfileViewModel+ 6c   ?? *SaveAsProfileCommand+6?   ?? 5      6   m? 5      $??Settings...??6    60   ?? *SettingsCommand+6V   ?? 5      6   m? 5      $??Exit...??6   6.   5      5      ?? 5   
   $??10,23,10,27q?6   .@?5      ?? $
??Main??6   .??5      ? .+?59      ?? $??Displays??6   $	??165??61   $	??Top=?6>   $??
10,10,10,0q?6|   .??5:      ?? $
??Auto=?6'   $
??Auto=?6L   $	??0,0q?6o   .??5;      ? -   5         ?Name$ Displays??5;   (   $??0,0,0,0q?6:   5<      5=      ?? 5?      $??Instruments??6   $??10,180,10,31q?64   .??5@      ? .+?5A      &   System.Windows.Controls.DataGrid  -   5          Name$ InstrumentsGrid??5A   '      AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  	   SelectionUnit$
	 Cell=?6Z  
   SelectionMode$
 Single=?6o  $??0,0,0,69q?6?   6@   ?? *InstrumentsProfileViewModel+??6t   ?? *Instruments+ ?? UpdateSourceTrigger$ PropertyChanged=? ?? SelectedItem 6?   ?? *SelectedIntrument+$ PropertyChanged=? ? 	Resources 5B   "   *  $System.Windows.Controls.DataGridCell ?? 
TargetType ?? Triggers P? Property P? Value ?? Property ?? Value   DataGridVerticalText??%?   '        &	  ?     ?? 5C   &   "  6,    5D   *   P? 5E   .    ?@ 
IsSelected . . ?5F   2   ??  ? ??
 ???d?6O   5G   /   5I   '   ?? 5K   &   " ??6I   .?5L   *   ??  ? ??$ Center=?6N   5M   '   5N   #      Columns 5O   "   4  .System.Windows.Controls.DataGridTemplateColumn  5P   &   ,  &System.Windows.Controls.DataGridColumn   Header$ Action??6=      Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 6M      CellTemplate 5Q   *   ?? 5R   .   .R?5S   2   ??  ??Name$ btnFindHandle??6:     
BlueButton#??[ 6Q   ??6u   ?? ??6?   ?? *
ButtonBg+??6?   ??  ?? Path$" DataContext.GetHandleCommand? ?? RelativeSource ?? *FindAncestor+ ?? AncestorType"  $??SELECT??5V   F   5W   3   5X   /   5Z   '   0  *System.Windows.Controls.DataGridTextColumn  5]   &   $ Handle??69      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6I   ?? *Handle+   	CellStyle 5^   *   ?? 5_   .   "  64   .?5`   2   ??  ? ??
 ?????6c   ?? 5b   2    ? ?? 5c   6   ?? 5d   :    ?? 
TargetType"  6J   .R?5e   >   ? #??z??6C   .+?5f   B   ?? $??Center=?6S   5g   ?   5h   ;   5j   3   5k   /   5m   '     5n   &   $ 
Instrument??69   $ * 6}       ElementStyle#  [  6?    6M   ?? *InstrumentName+! ?? Mode$! TwoWay=?6?     5o   &   $ Action??6=   $ 80 6M    5p   *   ?? 5q   .   .R?5r   2   ?? $ btnFindHandle??6:   #??[ 6Q   ??6u   ?? ??6?   ?? $  DataContext.ReadPosCommand? ?? *FindAncestor+"  $??READ POS??5u   F   5v   3   5w   /   5y   '     5z   &   $
 PosX??69   #  [  6    6G   ?? *PosX+$ 	LostFocus=?6?     5{   &   $
 PosY??69   #  [  6    6G   ?? *PosY+$ 	LostFocus=?6?     5|   &   $ Width??69   #  [  6?    6H   ?? *Width+$ 	LostFocus=?6?     5}   &   $ Height??69   #  [  6?    6I   ?? *Height+$ 	LostFocus=?6?   5?      ?? 5?      
??? E??$q??kClick on the Select button to choose an intsrument or press right button on the mouse to cancel the method!??6?   $
??Left=?6?   $??	14,0,0,44q?6?   $	??738??6?   $??20??6?   $??Bottom=?6?   ?? 5?      $	??Add??6%   #??[ 6?   $
??Left=?6?   $??67??6?   $
??Auto??6?   $??10,0,0,6q?6?   $??Bottom=?6?    63   ?? *InstrumentsProfileViewModel+??6g   ?? *AddInstrumentCommand+6?   5?      5?      5?      5?      ?? 5?      $
??Logs??6   .??5?      ? .+?5?      ?? -   5        " ??Name$" 	WSClients??5?      "# ?? VerticalScrollBarVisibility$
# Auto=?61   $$ ?? HorizontalScrollBarVisibility$
$ Auto=?6T   $??Courier New)?6z   $??16(?6?   $??
10,10,10,0q?6?   $	??128??6?   $	??Top=?6?   6?   ?? 5?      -   5        $" 
LogTextBox??5?      $
# Auto=?6W   $
$ Auto=?6z   $??Courier New)?6?   $??16(?6?   $??10,143,10,47q?6  ??6?   ?? $ LogViewModel.LogText?% ?? FallbackValue$	% LOG??6  ?? 5?      $??Clear??6   $??Right=?6d   $??	0,0,10,10q?6?   $??Bottom=?6?   $	??131??6?   ??6-   ?? $" LogViewModel.LogClearCommand?6?   5?      5?      5?      ?? 5?   
   $
??Auto??6   $??Bottom=?6"   .@?5?      ?? $	??HHH??6   6-   5?      5?      5?      