<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupdmember_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetail">
          <td colspan="6">{lbl:custInfo}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:UpdMultiple}</td>
          <td class="colW2">
            <xsl:value-of select="@updmultiple" />
          </td>
          <td class="colW1">{lbl:Updpt}</td>
          <td class="colW2">
            <xsl:value-of select="@updpt" />
          </td>
          <td class="colW1">{lbl:Updptv2}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv2" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:Updptv3}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv3" />
          </td>
          <td class="colW1">{lbl:Updptv5}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv5" />
          </td>
          <td class="colW1">{lbl:Updptv8}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv8" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:Updptv10}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv10" />
          </td>
          <td class="colW1">{lbl:Updptv11}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv11" />
          </td>
          <td class="colW1">{lbl:Updptv99}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv99" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:Updptv99MP}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv99mp" />
          </td>
          <td class="colW1">{lbl:Updptv161}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv161" />
          </td>
          <td class="colW1">{lbl:SubAccName}</td>
          <td class="colW2">
            <xsl:value-of select="@subaccname" />
          </td>          
        </tr>
        <tr >
          <td class="colW1">{lbl:IP}</td>
          <td class="colW2">
            <xsl:value-of select="@ip" />
          </td>
          <td class="colW1">{lbl:Language}</td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
          <td class="colW1">{lbl:ErrCode}</td>
          <td class="colW2">
            <xsl:value-of select="@errcode" />
          </td>          
        </tr>
        <tr >          
          <td class="colW1">{lbl:ErrMsg}</td>
          <td class="colW2">
            <xsl:value-of select="@errmsg" />
          </td>
          <td class="colTitle"></td>
          <td class="colValue"></td>
          <td class="colTitle"></td>
          <td class="colValue"></td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="ag">
        <tr class="headerDetail" onclick="$('#ag{{tbodylogid}}').slideToggle();">
          <td colspan="6" >{lbl:ag}</td>
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
                  <td colspan="6">{lbl:MixSportsParlay}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v99spt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_9" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue"></td>
                  <td class="colTitle"></td>
                  <td class="colValue"></td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:NumberGame}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v161spt_81}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_81" />
                  </td>
                  <td class="colTitle">{lbl:v161spt_83}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_83" />
                  </td>
                  <td class="colTitle">{lbl:v161spt_85}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_85" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v161spt_86}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_86" />
                  </td>
                  <td class="colTitle">{lbl:v161slivept_85}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v161alivept_85" />
                  </td>
                  <td class="colTitle">{lbl:v161slivept_86}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v161alivept_86" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v161slivept_87}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v161alivept_87" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue"></td>
                  <td class="colTitle"></td>
                  <td class="colValue"></td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>



</xsl:stylesheet>
