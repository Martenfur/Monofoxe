<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.2.2" name="testTileset" tilewidth="32" tileheight="32" tilecount="16" columns="4">
 <image source="testTileset.png" width="128" height="128"/>
 <terraintypes>
  <terrain name="New Terrain" tile="5"/>
 </terraintypes>
 <tile id="0" terrain=",,,0">
  <objectgroup draworder="index">
   <object id="1" x="-21" y="-19"/>
   <object id="2" x="-21" y="30">
    <ellipse/>
   </object>
   <object id="3" template="Objects/Point.tx" x="25" y="-12"/>
  </objectgroup>
 </tile>
 <tile id="1" terrain=",,0,0"/>
 <tile id="2" terrain=",,0,"/>
 <tile id="5" terrain=",0,,0"/>
 <tile id="6" terrain="0,0,0,0"/>
 <tile id="8" terrain="0,,0,"/>
 <tile id="10" terrain=",0,,"/>
 <tile id="12" terrain="0,0,,"/>
 <tile id="13" terrain="0,,,"/>
</tileset>
