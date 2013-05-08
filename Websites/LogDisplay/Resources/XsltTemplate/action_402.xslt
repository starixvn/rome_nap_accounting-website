<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerinsmember_mag">
        <table class="tblDesc">

            <xsl:for-each select="ag">
                <tr class="headerDetail" onclick="$('#ag{{tbodylogid}}').slideToggle();">
                    <td>{lbl:ag}</td>
                </tr>
                <tr>
                    <td>
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
                            <td class="colValue"></td>
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
                            <td class="colTitle">{lbl:v99spt_9}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v99apt_9" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue"></td>
                            <td class="colTitle"></td>
                            <td class="colValue"></td>
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
            <xsl:for-each select="m">
                <tr class="headerDetail" onclick="$('#m{{tbodylogid}}').slideToggle();">
                    <td >{lbl:m}</td>
                </tr>
                <tr>
                    <td>
                      <div style="display:none;" id="m{{tbodylogid}}">
                        <table class="tblDesc">
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:soccer}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:mpt_4}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_4" />
                            </td>
                            <td class="colTitle">{lbl:mpt_5}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_5" />
                            </td>
                            <td class="colTitle">{lbl:mpt_6}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_6" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:mpt_7}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_7" />
                            </td>
                            <td class="colTitle">{lbl:mpt_8}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_8" />
                            </td>
                            <td class="colTitle">{lbl:mpt_9}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_9" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:mpt_10}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mpt_10" />
                            </td>
                            <td class="colTitle">{lbl:mlivept_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mlivept_1" />
                            </td>
                            <td class="colTitle">{lbl:mlivept_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mlivept_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:mlivept_7}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mlivept_7" />
                            </td>
                            <td class="colTitle">{lbl:mlivept_8}</td>
                            <td class="colValue">
                              <xsl:value-of select="@mlivept_8" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:basketball}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v2mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v2mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:v2mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v2mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:v2mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v2mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v2mpt_7}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v2mpt_7" />
                            </td>
                            <td class="colTitle">{lbl:v2mpt_8}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v2mpt_8" />
                            </td>
                            <td class="colTitle">{lbl:v2mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v2mpt_20" />
                            </td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:football}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v3mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v3mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:v3mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v3mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:v3mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v3mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v3mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v3mpt_20" />
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
                            <td class="colTitle">{lbl:v5mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v5mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:v5mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v5mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:v5mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v5mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v5mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v5mpt_20" />
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
                            <td class="colTitle">{lbl:v8mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v8mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:v8mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v8mpt_3" />
                            </td>
                            <td class="colTitle">{lbl:v8mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v8mpt_20" />
                            </td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:golf}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v10mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v10mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:v10mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v10mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:v10mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v10mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v10mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v10mpt_20" />
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
                            <td class="colTitle">{lbl:v11mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v11mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:v11mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v11mpt_20" />
                            </td>
                            <td colspan="2"></td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:othersports}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v99mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v99mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:v99mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v99mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:v99mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v99mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:v99mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@v99mpt_20" />
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
            <xsl:for-each select="aum" >
                <tr class="headerDetail" onclick="$('#aum{{tbodylogid}}').slideToggle();">
                    <td colspan="6">{lbl:aum}</td>
                </tr>
                <tr>
                    <td>
                      <div style="display:none;" id="aum{{tbodylogid}}">
                        <table class="tblDesc">
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:soccer}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:a_mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_mpt_4}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_4" />
                            </td>
                            <td class="colTitle">{lbl:a_mpt_5}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_5" />
                            </td>
                            <td class="colTitle">{lbl:a_mpt_6}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_6" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_mpt_7}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_7" />
                            </td>
                            <td class="colTitle">{lbl:a_mpt_8}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_8" />
                            </td>
                            <td class="colTitle">{lbl:apt_9}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_9" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_mpt_10}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mpt_10" />
                            </td>
                            <td class="colTitle">{lbl:a_mlivept_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mlivept_1" />
                            </td>
                            <td class="colTitle">{lbl:a_mlivept_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mlivept_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:alivept_7}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mlivept_7" />
                            </td>
                            <td class="colTitle">{lbl:alivept_8}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_mlivept_8" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:basketball}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v2mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v2mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_v2mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v2mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:a_v2mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v2mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v2mpt_7}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v2mpt_7" />
                            </td>
                            <td class="colTitle">{lbl:a_v2mpt_8}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v2mpt_8" />
                            </td>
                            <td class="colTitle">{lbl:a_v2mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v2mpt_20" />
                            </td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:football}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v3mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v3mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_v3mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v3mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:a_v3mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v3mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v3mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v3mpt_20" />
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
                            <td class="colTitle">{lbl:a_v5mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v5mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_v5mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v5mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:a_v5mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v5mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v5mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v5mpt_20" />
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
                            <td class="colTitle">{lbl:a_v8mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v8mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_v8mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v8mpt_3" />
                            </td>
                            <td class="colTitle">{lbl:a_v8mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v8mpt_20" />
                            </td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:golf}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v10mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v10mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_v10mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v10mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:a_v10mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v10mpt_3" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v10mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v10mpt_20" />
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
                            <td class="colTitle">{lbl:a_v11mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v11mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_v11mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v11mpt_20" />
                            </td>
                            <td colspan="2"></td>
                          </tr>
                          <tr class="headerSportTypeDetail">
                            <td colspan="6">{lbl:othersports}</td>
                          </tr>
                          <tr >
                            <td class="colTitle">{lbl:a_v99mpt_1}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v99mpt_1" />
                            </td>
                            <td class="colTitle">{lbl:a_v99mpt_2}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v99mpt_2" />
                            </td>
                            <td class="colTitle">{lbl:a_v99mpt_3}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v99mpt_3" />
                            </td>
                          </tr>
                          <tr >

                            <td class="colTitle">{lbl:a_v99mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@a_v99mpt_20" />
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
                              <xsl:value-of select="@a_v99mpt_9" />
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
                              <xsl:value-of select="@a_v161mpt_81" />
                            </td>
                            <td class="colW1">{lbl:v161spt_83}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v161mpt_83" />
                            </td>
                            <td class="colW1">{lbl:v161spt_85}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v161mpt_85" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colW1">{lbl:v161spt_86}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v161mpt_86" />
                            </td>
                            <td class="colW1">{lbl:v161slivept_85}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v161mlivept_85" />
                            </td>
                            <td class="colW1">{lbl:v161slivept_86}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v161mlivept_86" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colW1">{lbl:v161slivept_87}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v161mlivept_87" />
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
            <xsl:for-each select="iam">
                <tr class="headerDetail" onclick="$('#iam{{tbodylogid}}').slideToggle();">
                    <td >{lbl:iam}</td>
                </tr>
                <tr>
                    <td>
                      <div style="display:none;" id="iam{{tbodylogid}}">
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
                            <td class="colTitle">{lbl:isa_v2mpt_20}</td>
                            <td class="colValue">
                              <xsl:value-of select="@isa_v2mpt_20" />
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

                          <tr class="headerSportTypeDetail">
                            <td colspan="6">
                              {lbl:MixSportsParlay}
                            </td>
                          </tr>
                          <tr >
                            <td class="colW1">{lbl:v99spt_9}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v99mpt_9" />
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
                              <xsl:value-of select="@isa_v161mpt_81" />
                            </td>
                            <td class="colW1">{lbl:v161spt_83}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v161mpt_83" />
                            </td>
                            <td class="colW1">{lbl:v161spt_85}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v161mpt_85" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colW1">{lbl:v161spt_86}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v161mpt_86" />
                            </td>
                            <td class="colW1">{lbl:v161slivept_85}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v161mlivept_85" />
                            </td>
                            <td class="colW1">{lbl:v161slivept_86}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v161mlivept_86" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colW1">{lbl:v161slivept_87}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v161mlivept_87" />
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
