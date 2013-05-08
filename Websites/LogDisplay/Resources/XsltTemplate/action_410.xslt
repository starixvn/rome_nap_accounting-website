<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupdsuper_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetail">
          <td colspan="6">{lbl:custInfo}</td>
        </tr>
        <tr>
          <td class="colW1">{lbl:IP}</td>
          <td class="colW2">
            <xsl:value-of select="@ip" />
          </td>
          <td class="colW1">{lbl:ErrCode}</td>
          <td class="colW2">
            <xsl:value-of select="@errcode" />
          </td>
          <td class="colW1">{lbl:Language}</td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
        </tr>
        <tr>

          <td class="colW1">{lbl:ErrMsg}</td>
          <td class="colW2" >
            <xsl:value-of select="@errmsg" />
          </td>
          <td class="colW1"></td>
          <td class="colW2" >

          </td>
          <td class="colW1"></td>
          <td class="colW2" >

          </td>
        </tr>
      </xsl:for-each>
      <xsl:for-each select="s">
        <tr class="headerDetail" onclick="$('#s{{tbodylogid}}').slideToggle();" id="header{{tbodylogid}}" >
          <td colspan="6">{lbl:s}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:none;" id="s{{tbodylogid}}">
              <table  class="tblDesc">
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:soccer}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_1" />
                  </td>
                  <td class="colW1">{lbl:spt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_2" />
                  </td>
                  <td class="colW1">{lbl:spt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:spt_4}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_4" />
                  </td>
                  <td class="colW1">{lbl:spt_5}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_5" />
                  </td>
                  <td class="colW1">{lbl:spt_6}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_6" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:spt_7}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_7" />
                  </td>
                  <td class="colW1">{lbl:spt_8}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_8" />
                  </td>
                  <td class="colW1">{lbl:spt_9}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_9" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:spt_10}</td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_10" />
                  </td>




                  <td class="colW1">{lbl:slivept_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_1" />
                  </td>
                  <td class="colW1">{lbl:slivept_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:slivept_7}</td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_7" />
                  </td>

                  <td class="colW1">{lbl:slivept_8}</td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_8" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>


                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Basketball}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v2spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_1" />
                  </td>
                  <td class="colW1">{lbl:v2spt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_2" />
                  </td>
                  <td class="colW1">{lbl:v2spt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_3" />
                  </td>

                </tr>
                <tr >
                  <td class="colW1">{lbl:v2spt_7}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_7" />
                  </td>
                  <td class="colW1">{lbl:v2spt_8}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_8" />
                  </td>
                  <td class="colW1">{lbl:v2spt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Football}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v3spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_1" />
                  </td>
                  <td class="colW1">{lbl:v3spt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_2" />
                  </td>
                  <td class="colW1">{lbl:v3spt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v3spt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Tennis}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v5spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_1" />
                  </td>
                  <td class="colW1">{lbl:v5spt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_2" />
                  </td>
                  <td class="colW1">{lbl:v5spt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_3" />
                  </td>
                </tr>
                <tr >

                  <td class="colW1">{lbl:v5spt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Baseball}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v8spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v8spt_1" />
                  </td>
                  <td class="colW1">{lbl:v8spt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v8spt_3" />
                  </td>
                  <td class="colW1">{lbl:v8spt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v8spt_20" />
                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Golf}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v10spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_1" />
                  </td>
                  <td class="colW1">{lbl:v10spt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_2" />
                  </td>
                  <td class="colW1">{lbl:v10spt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">{lbl:v10spt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Motosports}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v11spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v11spt_1" />
                  </td>
                  <td class="colW1">{lbl:v11spt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v11spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:OtherSports}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v99spt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_1" />
                  </td>
                  <td class="colW1">{lbl:v99spt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_2" />
                  </td>
                  <td class="colW1">{lbl:v99spt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_3" />
                  </td>

                </tr>
                <tr >
                  <td class="colW1">{lbl:v99spt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:MixSportsParlay}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v99spt_9}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_9" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:NumberGame}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161spt_81}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_81" />
                  </td>
                  <td class="colW1">{lbl:v161spt_83}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_83" />
                  </td>
                  <td class="colW1">{lbl:v161spt_85}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_85" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161spt_86}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_86" />
                  </td>
                  <td class="colW1">{lbl:v161slivept_85}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161slivept_85" />
                  </td>
                  <td class="colW1">{lbl:v161slivept_86}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161slivept_86" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161slivept_87}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161slivept_87" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
    

      </xsl:for-each>
      <xsl:for-each select="m">
        <tr class="headerDetail" onclick="$('#m{{tbodylogid}}').slideToggle();">
          <td colspan="6">{lbl:m}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:none;" id="m{{tbodylogid}}">
              <table class="tblDesc">
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:soccer}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_1" />
                  </td>
                  <td class="colW1">{lbl:mpt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_2" />
                  </td>
                  <td class="colW1">{lbl:mpt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:mpt_4}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_4" />
                  </td>
                  <td class="colW1">{lbl:mpt_5}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_5" />
                  </td>
                  <td class="colW1">{lbl:mpt_6}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_6" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:mpt_7}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_7" />
                  </td>
                  <td class="colW1">{lbl:mpt_8}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_8" />
                  </td>
                  <td class="colW1">{lbl:mpt_9}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_9" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:mpt_10}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_10" />
                  </td>
                  <td class="colW1">{lbl:mlivept_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_1" />
                  </td>
                  <td class="colW1">{lbl:mlivept_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:mlivept_7}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_7" />
                  </td>
                  <td class="colW1">{lbl:mlivept_8}</td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_8" />
                  </td>

                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Basketball}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v2mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_1" />
                  </td>
                  <td class="colW1">{lbl:v2mpt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_2" />
                  </td>
                  <td class="colW1">{lbl:v2mpt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_3" />
                  </td>

                </tr>
                <tr >
                  <td class="colW1">{lbl:v2mpt_7}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_7" />
                  </td>
                  <td class="colW1">{lbl:v2mpt_8}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_8" />
                  </td>
                  <td class="colW1">{lbl:v2mpt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_20" />
                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Football}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v3mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_1" />
                  </td>
                  <td class="colW1">{lbl:v3mpt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_2" />
                  </td>
                  <td class="colW1">{lbl:v3mpt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v3mpt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Tennis}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v5mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_1" />
                  </td>
                  <td class="colW1">{lbl:v5mpt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_2" />
                  </td>
                  <td class="colW1">{lbl:v5mpt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v5mpt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Baseball}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v8mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v8mpt_1" />
                  </td>
                  <td class="colW1">{lbl:v8mpt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v8mpt_3" />
                  </td>
                  <td class="colW1">{lbl:v8mpt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v8mpt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Golf}
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:v10mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v10mpt_1" />
                  </td>
                  <td class="colW1">{lbl:v10mpt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v10mpt_2" />
                  </td>
                  <td class="colW1">{lbl:v10mpt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v10mpt_3" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:Motosports}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v11mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v11mpt_1" />
                  </td>
                  <td class="colW1">{lbl:v11mpt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v11mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:OtherSports}
                  </td>
                </tr>
                <tr >

                  <td class="colW1">{lbl:v99mpt_1}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_1" />
                  </td>
                  <td class="colW1">{lbl:v99mpt_2}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_2" />
                  </td>
                  <td class="colW1">{lbl:v99mpt_3}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_3" />
                  </td>
                </tr>
                <tr >

                  <td class="colW1">{lbl:v99mpt_20}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:MixSportsParlay}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v99spt_9}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_9" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:NumberGame}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161spt_81}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_81" />
                  </td>
                  <td class="colW1">{lbl:v161spt_83}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_83" />
                  </td>
                  <td class="colW1">{lbl:v161spt_85}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_85" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161spt_86}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_86" />
                  </td>
                  <td class="colW1">{lbl:v161slivept_85}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mlivept_85" />
                  </td>
                  <td class="colW1">{lbl:v161slivept_86}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mlivept_86" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161slivept_87}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mlivept_87" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>
              </table>
            </div>
          </td>
        </tr>

      </xsl:for-each>  
      <xsl:for-each select="ag">
        <tr class="headerDetail" onclick="$('#ag{{tbodylogid}}').slideToggle();">
          <td colspan="6">{lbl:ag}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:none;" id="ag{{tbodylogid}}">
              <table class="tblDesc">
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:soccer}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_1" />
                  </td>
                  <td class="colTitle">{lbl:apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_2" />
                  </td>
                  <td class="colTitle">{lbl:apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_4}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_4" />
                  </td>
                  <td class="colTitle">{lbl:apt_5}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_5" />
                  </td>
                  <td class="colTitle">{lbl:apt_6}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_6" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_7}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_7" />
                  </td>
                  <td class="colTitle">{lbl:apt_8}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_8" />
                  </td>
                  <td class="colTitle">{lbl:apt_9}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_9" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_10}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_10" />
                  </td>
                  <td class="colTitle">{lbl:alivept_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_1" />
                  </td>
                  <td class="colTitle">{lbl:alivept_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:alivept_7}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_7" />
                  </td>
                  <td class="colTitle">{lbl:alivept_8}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_8" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:basketball}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v2apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v2apt_7}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_7" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_8}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_8" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:football}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v3apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v3apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v3apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v3apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:tennis}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v5apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v5apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v5apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v5apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:baseball}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v8apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v8apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_3" />
                  </td>
                  <td class="colTitle">{lbl:v8apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:golf}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v10apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v10apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v10apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v10apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:motorsports}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v11apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v11apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v11apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v11apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:othersports}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v99apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v99apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v99apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_3" />
                  </td>
                </tr>
                <tr >

                  <td class="colTitle">{lbl:v99apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:MixSportsParlay}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v99spt_9}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v99apt_9" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    {lbl:NumberGame}
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161spt_81}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161apt_81" />
                  </td>
                  <td class="colW1">{lbl:v161spt_83}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161apt_83" />
                  </td>
                  <td class="colW1">{lbl:v161spt_85}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161apt_85" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161spt_86}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161apt_86" />
                  </td>
                  <td class="colW1">{lbl:v161slivept_85}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161alivept_85" />
                  </td>
                  <td class="colW1">{lbl:v161slivept_86}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161alivept_86" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:v161slivept_87}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v161alivept_87" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>

    </table>
  </xsl:template>



</xsl:stylesheet>
