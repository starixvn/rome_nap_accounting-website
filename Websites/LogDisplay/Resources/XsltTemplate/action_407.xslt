<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmember_master">
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

                       
                        <td class="colW1">{lbl:allowupdpt}</td>
                        <td class="colW2">
                            <xsl:value-of select="@allowupdpt" />
                        </td>
                    </tr>

                    <tr>
                        <td class="colW1">{lbl:subaccid}</td>
                        <td class="colW2">
                            <xsl:value-of select="@subaccid" />
                        </td>
                        <td class="colW1">{lbl:ip}</td>
                        <td class="colW2">
                            <xsl:value-of select="@ip" />
                        </td>
                        <td class="colW1">{lbl:errcode}</td>
                        <td class="colW2">
                            <xsl:value-of select="@errcode" />
                        </td>
                    </tr>
                    <tr>
                        <td class="colW1">{lbl:errmsg}</td>
                        <td class="colW2">
                            <xsl:value-of select="@errmsg" />
                        </td>
                        <td class="colW1">{lbl:language}</td>
                        <td class="colW2">
                            <xsl:value-of select="@language" />
                        </td>
                        <td class="colTitle"></td>
                        <td class="colValue">

                        </td>
                    </tr>
                </table>

            </xsl:when>
            <xsl:otherwise>
                <table class="tblDesc">
                    <xsl:for-each select="m">
                        <tr class="headerDetail" onclick="$('#m{{tbodylogid}}').slideToggle();">
                            <td colspan="6">{lbl:m}</td>
                        </tr>
                        <tr>
                            <td colspan="6" >
                              <div id="m{{tbodylogid}}" style="display:block;">
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
                  <!--
                      <xsl:for-each select="iam">
                        <tr class="headerDetail" onclick="$('#iam{{tbodylogid}}').slideToggle();">
                          <td >{lbl:iam}</td>
                        </tr>
                        <tr>
                          <td>
                            <div id="iam{{tbodylogid}}" style="display:none;">
                              <table class="tblDesc">
                                <tr class="headerSportTypeDetail">
                                  <td colspan="6">{lbl:soccer}</td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mpt_2}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_2" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mpt_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_3" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_mpt_4}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_4" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mpt_5}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_5" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mpt_6}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_6" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_mpt_7}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_7" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mpt_8}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_8" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mpt_9}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_9" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_mpt_10}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mpt_10" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mlivept_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mlivept_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mlivept_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mlivept_3" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_mlivept_7}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mlivept_7" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_mlivept_8}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_mlivept_8" />
                                  </td>
                                  <td class="colTitle"></td>
                                  <td class="colValue">

                                  </td>
                                </tr>

                                <tr class="headerSportTypeDetail">
                                  <td colspan="6">{lbl:basketball}</td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v2mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v2mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v2mpt_2}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v2mpt_2" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v2mpt_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v2mpt_3" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v2mpt_7}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v2mpt_7" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v2mpt_8}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v2mpt_8" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v2mpt_9}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v2mpt_9" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v2mpt_20}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v2mpt_20" />
                                  </td>
                                  <td class="colTitle"></td>
                                  <td class="colValue">

                                  </td>
                                  <td class="colTitle"></td>
                                  <td class="colValue">

                                  </td>
                                </tr>


                                <tr class="headerSportTypeDetail">
                                  <td colspan="6">{lbl:football}</td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v3mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v3mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v3mpt_2}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v3mpt_2" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v3mpt_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v3mpt_3" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v3mpt_20}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v3mpt_20" />
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
                                  <td class="colTitle">{lbl:isa_v5mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v5mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v5mpt_2}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v5mpt_2" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v5mpt_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v5mpt_3" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v5mpt_20}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v5mpt_20" />
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
                                  <td class="colTitle">{lbl:isa_v8mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v8mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v8mpt_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v8mpt_3" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v8mpt_20}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v8mpt_20" />
                                  </td>
                                </tr>


                                <tr class="headerSportTypeDetail">
                                  <td colspan="6">{lbl:golf}</td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v10mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v10mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v10mpt_2}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v10mpt_2" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v10mpt_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v10mpt_3" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v10mpt_20}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v10mpt_20" />
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
                                  <td class="colTitle">{lbl:isa_v11mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v11mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v11mpt_20}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v11mpt_20" />
                                  </td>
                                  <td class="colTitle"></td>
                                  <td class="colValue">

                                  </td>
                                </tr>

                                <tr class="headerSportTypeDetail">
                                  <td colspan="6">{lbl:othersports}</td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v99mpt_1}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v99mpt_1" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v99mpt_2}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v99mpt_2" />
                                  </td>
                                  <td class="colTitle">{lbl:isa_v99mpt_3}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v99mpt_3" />
                                  </td>
                                </tr>
                                <tr >
                                  <td class="colTitle">{lbl:isa_v99mpt_20}</td>
                                  <td class="colValue">
                                    <xsl:value-of select="@isa_v99mpt_20" />
                                  </td>
                                  <td class="colTitle"></td>
                                  <td class="colValue">

                                  </td>
                                  <td class="colTitle"></td>
                                  <td class="colValue">

                                  </td>
                                </tr>
                              </table>
                            </div>
                          </td>
                        </tr>


                      </xsl:for-each>
                    -->
                  <xsl:for-each select="iame">
                    <tr class="headerDetail" onclick="$('#iame{{tbodylogid}}').slideToggle();">
                      <td >{lbl:iame}</td>
                    </tr>
                    <tr>
                      <td>
                        <div id="iame{{tbodylogid}}" style="display:none;">
                          <table class="tblDesc">
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:soccer}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:isa_mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_mpt_mem_4}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_4" />
                              </td>
                              <td class="colW1">{lbl:isa_mpt_mem_5}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_5" />
                              </td>
                              <td class="colW1">{lbl:isa_mpt_mem_6}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_6" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_mpt_mem_7}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_7" />
                              </td>
                              <td class="colW1">{lbl:isa_mpt_mem_8}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_8" />
                              </td>
                              <td class="colW1">{lbl:isa_mpt_mem_9}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_9" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_mpt_mem_10}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mpt_mem_10" />
                              </td>
                              <td class="colW1">{lbl:isa_mlivept_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mlivept_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_mlivept_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mlivept_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_mlivept_mem_7}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mlivept_mem_7" />
                              </td>
                              <td class="colW1">{lbl:isa_mlivept_mem_8}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_mlivept_mem_8" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                                
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:basketball}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v2mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v2mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_v2mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v2mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:isa_v2mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v2mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              
                              <td class="colW1">{lbl:isa_v2mpt_mem_7}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v2mpt_mem_7" />
                              </td>
                              <td class="colW1">{lbl:isa_v2mpt_mem_8}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v2mpt_mem_8" />
                              </td>
                              <td class="colW1">{lbl:isa_v2mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v2mpt_mem_20" />
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:football}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v3mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v3mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_v3mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v3mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:isa_v3mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v3mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v3mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v3mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:tennis}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v5mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v5mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_v5mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v5mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:isa_v5mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v5mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v5mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v5mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:baseball}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v8mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v8mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_v8mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v8mpt_mem_3" />
                              </td>
                              <td class="colW1">{lbl:isa_v8mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v8mpt_mem_20" />
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:golf}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v10mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v10mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_v10mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v10mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:isa_v10mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v10mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v10mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v10mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:motorsports}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v11mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v11mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_v11mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v11mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:othersports}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v99mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v99mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:isa_v99mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v99mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:isa_v99mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v99mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:isa_v99mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v99mpt_mem_20" />
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
                                <xsl:value-of select="@isa_v99mpt_mem_9" />
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
                                <xsl:value-of select="@isa_v161mpt_mem_81" />
                              </td>
                              <td class="colW1">{lbl:v161spt_83}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v161mpt_mem_83" />
                              </td>
                              <td class="colW1">{lbl:v161spt_85}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v161mpt_mem_85" />
                              </td>
                            </tr>
                            <tr >
                              <td class="colW1">{lbl:v161spt_86}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v161mpt_mem_86" />
                              </td>
                              <td class="colW1">{lbl:v161slivept_85}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v161mlivept_mem_85" />
                              </td>
                              <td class="colW1">{lbl:v161slivept_86}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v161mlivept_mem_86" />
                              </td>
                            </tr>
                            <tr >
                              <td class="colW1">{lbl:v161slivept_87}</td>
                              <td class="colW2">
                                <xsl:value-of select="@isa_v161mlivept_mem_87" />
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
                  <xsl:for-each select="aume">
                    <tr class="headerDetail" onclick="$('#aume{{tbodylogid}}').slideToggle();">
                      <td >{lbl:aume}</td>
                    </tr>
                    <tr>
                      <td>
                        <div id="aume{{tbodylogid}}" style="display:none;">
                          <table class="tblDesc">
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:soccer}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:a_mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_mpt_mem_4}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_4" />
                              </td>
                              <td class="colW1">{lbl:a_mpt_mem_5}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_5" />
                              </td>
                              <td class="colW1">{lbl:a_mpt_mem_6}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_6" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_mpt_mem_7}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_7" />
                              </td>
                              <td class="colW1">{lbl:a_mpt_mem_8}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_8" />
                              </td>
                              <td class="colW1">{lbl:a_mpt_mem_9}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_9" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_mpt_mem_10}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mpt_mem_10" />
                              </td>
                              <td class="colW1">{lbl:a_mlivept_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mlivept_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_mlivept_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mlivept_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_mlivept_mem_7}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mlivept_mem_7" />
                              </td>
                              <td class="colW1">{lbl:a_mlivept_mem_8}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_mlivept_mem_8" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:basketball}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v2mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v2mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_v2mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v2mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:a_v2mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v2mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              
                              <td class="colW1">{lbl:a_v2mpt_mem_7}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v2mpt_mem_7" />
                              </td>
                              <td class="colW1">{lbl:a_v2mpt_mem_8}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v2mpt_mem_8" />
                              </td>
                              <td class="colW1">{lbl:a_v2mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v2mpt_mem_20" />
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:football}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v3mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v3mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_v3mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v3mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:a_v3mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v3mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v3mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v3mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:tennis}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v5mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v5mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_v5mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v5mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:a_v5mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v5mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v5mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v5mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:baseball}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v8mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v8mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_v8mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v8mpt_mem_3" />
                              </td>
                              <td class="colW1">{lbl:a_v8mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v8mpt_mem_20" />
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:golf}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v10mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v10mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_v10mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v10mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:a_v10mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v10mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v10mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v10mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:motorsports}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v11mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v11mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_v11mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v11mpt_mem_20" />
                              </td>
                              <td class="colW1"></td>
                              <td class="colW2">
                              </td>
                            </tr>
                            <tr class="headerSportTypeDetail">
                              <td colspan="6">{lbl:othersports}</td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v99mpt_mem_1}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v99mpt_mem_1" />
                              </td>
                              <td class="colW1">{lbl:a_v99mpt_mem_2}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v99mpt_mem_2" />
                              </td>
                              <td class="colW1">{lbl:a_v99mpt_mem_3}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v99mpt_mem_3" />
                              </td>
                            </tr>
                            <tr>
                              <td class="colW1">{lbl:a_v99mpt_mem_20}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v99mpt_mem_20" />
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
                                <xsl:value-of select="@a_v99mpt_mem_9" />
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
                                <xsl:value-of select="@a_v161mpt_mem_81" />
                              </td>
                              <td class="colW1">{lbl:v161spt_83}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v161mpt_mem_83" />
                              </td>
                              <td class="colW1">{lbl:v161spt_85}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v161mpt_mem_85" />
                              </td>
                            </tr>
                            <tr >
                              <td class="colW1">{lbl:v161spt_86}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v161mpt_mem_86" />
                              </td>
                              <td class="colW1">{lbl:v161slivept_85}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v161mlivept_mem_85" />
                              </td>
                              <td class="colW1">{lbl:v161slivept_86}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v161mlivept_mem_86" />
                              </td>
                            </tr>
                            <tr >
                              <td class="colW1">{lbl:v161slivept_87}</td>
                              <td class="colW2">
                                <xsl:value-of select="@a_v161mlivept_mem_87" />
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
            </xsl:otherwise>
        </xsl:choose>

    </xsl:template>



</xsl:stylesheet>
