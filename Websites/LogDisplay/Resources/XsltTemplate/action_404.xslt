<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupdmember_super">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1">{lbl:closed}</td>
            <td class="colW2">
              <xsl:value-of select="@closed" />
            </td>
            <td class="colW1">{lbl:suspended}</td>
            <td class="colW2">
              <xsl:value-of select="@suspended" />
            </td>
            <td class="colW1">{lbl:updatemultiple}</td>
            <td class="colW2">
              <xsl:value-of select="@updatemultiple" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:disabledfi}</td>
            <td class="colW2">
              <xsl:value-of select="@disabledfi" />
            </td>

            <td class="colW1">{lbl:subaccid}</td>
            <td class="colW2">
              <xsl:value-of select="@subaccid" />
            </td>
            <td class="colW1">{lbl:ip}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:errcode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">{lbl:errmsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
            <td class="colW1">{lbl:language}</td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
          </tr>
        </table>

      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
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

          <xsl:for-each select="au">
            <tr class="headerDetail" onclick="$('#au{{tbodylogid}}').slideToggle();">
              <td colspan="6">{lbl:au}</td>
            </tr>
            <tr>
              <td colspan="6">
                <div style="display:none;" id="au{{tbodylogid}}">
                  <table class="tblDesc">
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        {lbl:soccer}
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_1" />
                      </td>
                      <td class="colW1">{lbl:a_spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_2" />
                      </td>
                      <td class="colW1">{lbl:a_spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_spt_4}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_4" />
                      </td>
                      <td class="colW1">{lbl:a_spt_5}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_5" />
                      </td>
                      <td class="colW1">{lbl:a_spt_6}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_6" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_spt_7}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_7" />
                      </td>
                      <td class="colW1">{lbl:a_spt_8}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_8" />
                      </td>
                      <td class="colW1">{lbl:a_spt_9}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_spt_10}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_10" />
                      </td>
                      <td class="colW1">{lbl:a_slivept_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_slivept_1" />
                      </td>
                      <td class="colW1">{lbl:a_slivept_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_slivept_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_slivept_7}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_slivept_7" />
                      </td>
                      <td class="colW1">{lbl:a_slivept_8}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_slivept_8" />
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
                      <td class="colW1">{lbl:a_v2spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_1" />
                      </td>
                      <td class="colW1">{lbl:a_v2spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_2" />
                      </td>
                      <td class="colW1">{lbl:a_v2spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_v2spt_7}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_7" />
                      </td>
                      <td class="colW1">{lbl:a_v2spt_8}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_8" />
                      </td>
                      <td class="colW1">{lbl:a_v2spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_20" />
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        {lbl:Football}
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_v3spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3spt_1" />
                      </td>
                      <td class="colW1">{lbl:a_v3spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3spt_2" />
                      </td>
                      <td class="colW1">{lbl:a_v3spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_v3spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3spt_20" />
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

                      <td class="colW1">{lbl:a_v5spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5spt_1" />
                      </td>
                      <td class="colW1">{lbl:a_v5spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5spt_2" />
                      </td>
                      <td class="colW1">{lbl:a_v5spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5spt_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">{lbl:a_v5spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5spt_20" />
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


                      <td class="colW1">{lbl:a_v8spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8spt_1" />
                      </td>

                      <td class="colW1">{lbl:a_v8spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8spt_3" />
                      </td>
                      <td class="colW1">{lbl:a_v8spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8spt_20" />
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        {lbl:Golf}
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:a_v10spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10spt_1" />
                      </td>
                      <td class="colW1">{lbl:a_v10spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10spt_2" />
                      </td>
                      <td class="colW1">{lbl:a_v10spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10spt_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1">{lbl:a_v10spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10spt_20" />
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
                      <td class="colW1">{lbl:a_v11spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v11spt_1" />
                      </td>
                      <td class="colW1">{lbl:a_v11spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v11spt_20" />
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
                      <td class="colW1">{lbl:a_v99spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99spt_1" />
                      </td>
                      <td class="colW1">{lbl:a_v99spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99spt_2" />
                      </td>
                      <td class="colW1">{lbl:a_v99spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99spt_3" />
                      </td>

                    </tr>
                    <tr>
                      <td class="colW1">{lbl:a_v99spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99spt_20" />
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
                        <xsl:value-of select="@a_v99spt_9" />
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
                        <xsl:value-of select="@a_v161spt_81" />
                      </td>
                      <td class="colW1">{lbl:v161spt_83}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161spt_83" />
                      </td>
                      <td class="colW1">{lbl:v161spt_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161spt_85" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:v161spt_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161spt_86" />
                      </td>
                      <td class="colW1">{lbl:v161slivept_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161slivept_85" />
                      </td>
                      <td class="colW1">{lbl:v161slivept_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161slivept_86" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:v161slivept_87}</td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161slivept_87" />
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

          <xsl:for-each select="ia">
            <tr class="headerDetail" onclick="$('#ia{{tbodylogid}}').slideToggle();">
              <td colspan="6">{lbl:ia}</td>
            </tr>
            <tr>
              <td colspan="6">
                <div style="display:none;" id="ia{{tbodylogid}}">
                  <table class="tblDesc">
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        {lbl:soccer}
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_2" />
                      </td>
                      <td class="colW1">{lbl:isa_spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_spt_4}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_4" />
                      </td>
                      <td class="colW1">{lbl:isa_spt_5}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_5" />
                      </td>
                      <td class="colW1">{lbl:isa_spt_6}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_6" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_spt_7}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_7" />
                      </td>
                      <td class="colW1">{lbl:isa_spt_8}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_8" />
                      </td>
                      <td class="colW1">{lbl:isa_spt_9}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_spt_10}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_spt_10" />
                      </td>
                      <td class="colW1">{lbl:isa_slivept_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_slivept_1" />
                      </td>
                      <td class="colW1">{lbl:isa_slivept_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_slivept_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_slivept_7}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_slivept_7" />
                      </td>
                      <td class="colW1">{lbl:isa_slivept_8}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_slivept_8" />
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
                      <td class="colW1">{lbl:isa_v2spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_v2spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2spt_2" />
                      </td>
                      <td class="colW1">{lbl:isa_v2spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_v2spt_7}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2spt_7" />
                      </td>
                      <td class="colW1">{lbl:isa_v2spt_8}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2spt_8" />
                      </td>
                      <td class="colW1">{lbl:isa_v2spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2spt_20" />
                      </td>

                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        {lbl:Football}
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_v3spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_v3spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3spt_2" />
                      </td>
                      <td class="colW1">{lbl:isa_v3spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_v3spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3spt_20" />
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
                      <td class="colW1">{lbl:isa_v5spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_v5spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5spt_2" />
                      </td>
                      <td class="colW1">{lbl:isa_v5spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5spt_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1">{lbl:isa_v5spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5spt_20" />
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
                      <td class="colW1">{lbl:isa_v8spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v8spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_v8spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v8spt_3" />
                      </td>
                      <td class="colW1">{lbl:isa_v8spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v8spt_20" />
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        {lbl:Golf}
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_v10spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_v10spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10spt_2" />
                      </td>
                      <td class="colW1">{lbl:isa_v10spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10spt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_v10spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10spt_20" />
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
                      <td class="colW1">{lbl:isa_v11spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v11spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_v11spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v11spt_20" />
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
                      <td class="colW1">{lbl:isa_v99spt_1}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99spt_1" />
                      </td>
                      <td class="colW1">{lbl:isa_v99spt_2}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99spt_2" />
                      </td>
                      <td class="colW1">{lbl:isa_v99spt_3}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99spt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">{lbl:isa_v99spt_20}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99spt_20" />
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
                        <xsl:value-of select="@isa_v99spt_9" />
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
                        <xsl:value-of select="@isa_v161spt_81" />
                      </td>
                      <td class="colW1">{lbl:v161spt_83}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161spt_83" />
                      </td>
                      <td class="colW1">{lbl:v161spt_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161spt_85" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:v161spt_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161spt_86" />
                      </td>
                      <td class="colW1">{lbl:v161slivept_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161slivept_85" />
                      </td>
                      <td class="colW1">{lbl:v161slivept_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161slivept_86" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">{lbl:v161slivept_87}</td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161slivept_87" />
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
          <!--
              <xsl:for-each select="isau">
                <tr class="headerDetail"  onclick="$('#isau{{tbodylogid}}').slideToggle();">
                  <td colspan="6">{lbl:isau}</td>
                </tr>
                <tr>
                  <td colspan="6">
                    <div style="display:none;" id="isau{{tbodylogid}}">
                      <table class="tblDesc">
                        <tr class="headerSportTypeDetail">
                          <td colspan="6">
                            {lbl:soccer}
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_smpt_2}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_2" />
                          </td>
                          <td class="colW1">{lbl:isa_smpt_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_3" />
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_smpt_4}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_4" />
                          </td>
                          <td class="colW1">{lbl:isa_smpt_5}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_5" />
                          </td>
                          <td class="colW1">{lbl:isa_smpt_6}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_6" />
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_smpt_7}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_7" />
                          </td>
                          <td class="colW1">{lbl:isa_smpt_8}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_8" />
                          </td>
                          <td class="colW1">{lbl:isa_smpt_9}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_9" />
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_smpt_10}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smpt_10" />
                          </td>
                          <td class="colW1">{lbl:isa_smlivept_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smlivept_1" />
                          </td>
                          <td class="colW1">{lbl:isa_smlivept_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smlivept_3" />
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_smlivept_7}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smlivept_7" />
                          </td>
                          <td class="colW1">{lbl:isa_smlivept_8}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_smlivept_8" />
                          </td>
                          <td class="colTitle"></td>
                          <td class="colValue">

                          </td>
                        </tr>
                        <tr class="headerSportTypeDetail">
                          <td colspan="6">
                            {lbl:Basketball}
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v2smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v2smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_v2smpt_2}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v2smpt_2" />
                          </td>
                          <td class="colW1">{lbl:isa_v2smpt_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v2smpt_3" />
                          </td>


                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v2smpt_7}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v2smpt_7" />
                          </td>
                          <td class="colW1">{lbl:isa_v2smpt_8}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v2smpt_8" />
                          </td>
                          <td class="colW1">{lbl:isa_v2smpt_9}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v2smpt_9" />
                          </td>

                        </tr>
                        <tr>
                          <td class="colW1">{lbl:isa_v2smpt_20}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v2smpt_20" />
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
                            {lbl:Football}
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v3smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v3smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_v3smpt_2}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v3smpt_2" />
                          </td>
                          <td class="colW1">{lbl:isa_v3smpt_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v3smpt_3" />
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v3smpt_20}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v3smpt_20" />
                          </td>
                          <td class="colW1"></td>
                          <td class="colW2">

                          </td>
                          <td class="colW1"></td>
                          <td class="colW2">

                          </td>
                        </tr>
                        <tr>
                          <td class="colW1">{lbl:isa_v5smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v5smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_v5smpt_2}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v5smpt_2" />
                          </td>

                          <td class="colW1">{lbl:isa_v5smpt_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v5smpt_3" />
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v5smpt_20}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v5smpt_20" />
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
                          <td class="colW1">{lbl:isa_v8smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v8smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_v8smpt_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v8smpt_3" />
                          </td>
                          <td class="colW1">{lbl:isa_v8smpt_20}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v8smpt_20" />
                          </td>

                        </tr>
                        <tr class="headerSportTypeDetail">
                          <td colspan="6">
                            {lbl:Golf}
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v10smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v10smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_v10smpt_2}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v10smpt_2" />
                          </td>
                          <td class="colW1">{lbl:isa_v10smpt_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v10smpt_3" />
                          </td>
                        </tr>
                        <tr>
                          <td class="colW1">{lbl:isa_v10smpt_20}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v10smpt_20" />
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
                            {lbl:Motosports}
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v11smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v11smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_v11smpt_20}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v11smpt_20" />
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
                          <td class="colW1">{lbl:isa_v99smpt_1}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v99smpt_1" />
                          </td>
                          <td class="colW1">{lbl:isa_v99smpt_2}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v99smpt_2" />
                          </td>
                          <td class="colW1">{lbl:isa_v99smpt_3}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v99smpt_3" />
                          </td>
                        </tr>
                        <tr >
                          <td class="colW1">{lbl:isa_v99smpt_20}</td>
                          <td class="colW2">
                            <xsl:value-of select="@isa_v99smpt_20" />
                          </td>
                          <td class="colW1"></td>
                          <td class="colW2">

                          </td>
                          <td class="colW1"></td>
                          <td class="colW2">

                          </td>
                        </tr>
                      </table>
                    </div>
                  </td>

                </tr>


              </xsl:for-each>
            -->
          
        </table>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>



</xsl:stylesheet>
