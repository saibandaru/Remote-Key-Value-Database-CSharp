   M S B A M L   `   `   `  ����  WpfApplication2    WpfApplication2.Page3   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5        N� Title$  Page3��5      .��5      ]�  � 	Resources 5   
      buttonStyle �� Property �� Value  topButtonStyle��%�  &	        &	 �     �� 5      .�5      ��  � ��$ Tahoma)�66   �� 5	       � ��$ 14(�66   �� 5
       � ��$
 Bold"�66   �� 5       � ��
 �����66   5      �� 5      .�5      ��  � ��$ Tahoma)�66   �� 5       � ��$ 12(�66   �� 5       � ��$
 Bold"�66   �� 5       � �� �#eeeeee66   �� 5       � ��$ 10q�66   5      5      .+�5   
   �� ���#eeeeee$��75��6*   $	��Top=�66   .+�5      ]� .+�5      �� -   5        #��[ 5      $��Right=�6A   $��1q�6m   ���	#FFFFEEEE.��6�   Help6�   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� $��Tahoma)�6    $��20(�64   
����� �$
��bold"�6[    �� Padding$ 10q�6m   Key/Value Database6�   �� 5      -   5        $��	PageLabel��5      $��Center=�6-   $��	30,5,30,5q�6H   $	��304��6\   $��14(�6h   .��6v   Client6~   5      5      o� 5      
����� �.@�5      m� -   5        $��Database Connection��5      
�������$	��250��6g   $��	50,5,50,5q�6s   $��Center=�6�   m� 5       -   5        $��Insert/Edit/Delete Value ��5       
�������$	��250��6o   $��	50,5,50,5q�6{   $��Center=�6�   m� 5!      -   5        $��      Query Database��5!      
�������$��	50,5,50,5q�6P   $	��250��6d   $��Center=�6�   
���iii�6�   m� 5"      -   5        $��      Persist Database��5"      
�������$��	50,5,50,5q�6R   $	��250��6f   $��Center=�6�   5%      5&      �� 5(   
   
����� �$��25��6.   $ 
Horizontal=�6:   $��Bottom=�6S   .+�5)      �� $,��&Status: Query is executed successfully��6   
�������6E   5*      �� 5+   
   
�������$	��300��6(   $ 
Horizontal=�65   .+�5,      �� 
�������$	��300��6,   $	��450��69   $ Vertical=�6E   .+�5-      �� -   5         ��Name$ textBox1��5-      $��Tahoma)�6.   $��15(�6B   $
��Wrap=�6P   $7��1Please select the option from dropdown and search��6d    �� TextAlignment$ Center=�5.      $ 25q�61   �� 5/      
�������$ 
Horizontal=�62   $��0,10,0,0q�6K   .+�50      �� -   5        $ textBox2��50       $��Tahoma)�62   $��15(�6G   $
��Wrap=�6U   $��Select:��6i   $ Center=�51      $��80��61   $ 10q�6<   $��	10,0,10,0q�6I   �� 52      -	   5         ��Name$ comboBox��52      $	��350��61   $��30��6=   $��12(�6I   .@�53      �� 	 �� 
IsSelected	 . .��6:   #!Search for value object using Key6]   �� 54      .��6'   '%Search for children of particular Key6N   �� 55      .��6'   64Search for keys that start with some particular text6]   �� 56      .��6'   B@Search for value object using search text in name or description6i   �� 57      .��6'   64Search for keys that fall in specified time interval6]   58      59      �� 5:      
�������$ 
Horizontal=�62   $��0,70,0,0q�6K   .+�5;      �� -
   5        $ textBox_name��5;       $��Tahoma)�66   $��15(�6K   $
��Wrap=�6Y   $��Search Text:��6m   $ Center=�5<      $	��120��61   $ 10q�6=   $��	10,0,10,0q�6J   �� 5=      -   5        
 ��Name$
 textBlock_name��5=      $��Tahoma)�66   $��15(�6J    �� TextWrapping$
 Wrap=�6Y   $
��K122��6m    �� TextAlignment$ Center=�5>      $	��250��61   $��25��6=   |�. 6I   5?      �� 5@      -   5         ��Name$ button��5@      $��Search��6)   $��30��6:   $	��100��6F   $��0,20,0,0q�6S   
�������6z   5B      �� 5C      -   5        $��Canvas1��5C      $	��300��6%   $��Right=�62   $	��600��6I   .+�5D      �� -   5        $��Request��5D   4   $ Vertical=�6   $	��300��6C   
 �� Top$ 0��6P    �� Left$ 0��6_    K� ZIndex$ 1��6o   .+�5E      �� $��Tahoma)�6    $��Center=�64   $��15(�6Q   $
��bold"�6_   $ 10q�6q   Requested Field6�   �� 5F      -   5        $��listBox1��5F   A   $��1��6   $��10, 0, 10, 0q�6+   $	��250��6Q   .@�5G      �� .��6&   0.Requesting to connect to server localhost:90306V   �� 5H      .��6&   &$Requesting to add new key/value pair6L   �� 5I      .��6&   '%Requesting to delete a key/value pair6M   �� 5J      .��6&   #!Requesting to edit a value object6I   �� 5K      .��6&   ><Requesting to search for key that starts wih particular text6d   5L      5M      �� 5N      -   5        $��Responce��5N   4   $ Vertical=�6   $	��300��6D   $ 0��6Q   $	 310��6`   $ 2��6r   .+�5O      �� $��Tahoma)�6    $��Center=�64   $��15(�6Q   $
��bold"�6_   $ 10q�6q   Response Field6�   �� 5P      -   5        $��listBox3��5P   A   $��1��6   $��10, 0, 10, 0q�6+   $	��250��6Q   .@�5Q      �� .��6&   $"Connected to server localhost:90306J   �� 5R      .��6&   $"Value object added with key "K123"6J   �� 5S      .��6&   (&Performance: Insert executed in 100 ms6N   �� 5T      .��6&   " Deleted an eement with key K12346H   �� 5U      .��6&   (&Performance: Delete executed in 100 ms6N   �� 5V      .��6&   !Edited an elment with key K12346G   �� 5W      .��6&   &$Performance: Edit executed in 130 ms6L   �� 5X      .��6&   Keys starting with 'K1' are6C   �� 5Y      .��6&   K123,K1234,K14567   �� 5Z      .��6&   '%Performance: Query executed in 200 ms6M   5[      5\      5]      5^      5_      5`      