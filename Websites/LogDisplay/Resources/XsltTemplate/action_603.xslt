<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerinsmember_ag">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1">{lbl:fname}</td>
            <td class="colW2">
              <xsl:value-of select="@fname" />
            </td>
            <td class="colW1">{lbl:lname}</td>
            <td class="colW2">
              <xsl:value-of select="@lname" />
            </td>
            <td class="colW1">{lbl:username}</td>
            <td class="colW2">
              <xsl:value-of select="@username" />
            </td>
          </tr>
          <tr>


            <td class="colW1">{lbl:phone}</td>
            <td class="colW2">
              <xsl:value-of select="@phone" />
            </td>
            <td class="colW1">{lbl:mphone}</td>
            <td class="colW2">
              <xsl:value-of select="@mphone" />
            </td>


            <td class="colW1">{lbl:fax}</td>
            <td class="colW2">
              <xsl:value-of select="@fax" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:answer}</td>
            <td class="colW2">
              <xsl:value-of select="@answer" />
            </td>

            <td class="colW1">{lbl:credit}</td>
            <td class="colW2">
              <xsl:value-of select="@credit" />
            </td>


            <td class="colW1">{lbl:ugroup}</td>
            <td class="colW2">
              <xsl:value-of select="@ugroup" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:IP}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>

            <td class="colW1">{lbl:success}</td>
            <td class="colW2">
              <xsl:value-of select="@success" />
            </td>
            <td class="colW1">{lbl:Language}</td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:SubAccName}</td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>

            <td class="colW1">{lbl:ErrCode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">{lbl:ErrMsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
          </tr>

          <xsl:choose>
            <xsl:when test="@minb">
              <tr>
                <td class="colW1">{lbl:discount}</td>
                <td class="colW2">
                  <xsl:value-of select="@discount" />
                </td>

                <td class="colW1">{lbl:discountcs}</td>
                <td class="colW2">
                  <xsl:value-of select="@discountcs" />
                </td>
                <td class="colW1">{lbl:discount1x2}</td>
                <td class="colW2">
                  <xsl:value-of select="@discount1x2" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:playerdiscount}</td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscount" />
                </td>

                <td class="colW1">{lbl:playerdiscountcs}</td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscountcs" />
                </td>
                <td class="colW1">{lbl:playerdiscount1x2}</td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscount1x2" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minb}</td>
                <td class="colW2">
                  <xsl:value-of select="@minb" />
                </td>
                <td class="colW1">{lbl:maxb}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxb" />
                </td>
                <td class="colW1">{lbl:mpm}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpm" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minbv2}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv2" />
                </td>
                <td class="colW1">{lbl:maxbv2}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv2" />
                </td>
                <td class="colW1">{lbl:mpmv2}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv2" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minbv3}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv3" />
                </td>
                <td class="colW1">{lbl:maxbv3}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv3" />
                </td>
                <td class="colW1">{lbl:mpmv3}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv3" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minbv5}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv5" />
                </td>
                <td class="colW1">{lbl:maxbv5}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv5" />
                </td>
                <td class="colW1">{lbl:mpmv5}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv5" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minbv8}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv8" />
                </td>
                <td class="colW1">{lbl:maxbv8}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv8" />
                </td>
                <td class="colW1">{lbl:mpmv8}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv8" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minbv10}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv10" />
                </td>
                <td class="colW1">{lbl:maxbv10}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv10" />
                </td>
                <td class="colW1">{lbl:mpmv10}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv10" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minbv11}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv11" />
                </td>
                <td class="colW1">{lbl:maxbv11}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv11" />
                </td>
                <td class="colW1">{lbl:mpmv11}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv11" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:minbv99}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv99" />
                </td>
                <td class="colW1">{lbl:maxbv99}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv99" />
                </td>
                <td class="colW1">{lbl:mpmv99}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv99" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:closed}</td>
                <td class="colW2">
                  <xsl:value-of select="@closed" />
                </td>
                <td class="colW1">{lbl:suspended}</td>
                <td class="colW2">
                  <xsl:value-of select="@suspended" />
                </td>
                <td class="colW1">{lbl:disablehorseracing}</td>
                <td class="colW2">
                  <xsl:value-of select="@disablehorseracing" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:disablefinancial}</td>
                <td class="colW2">
                  <xsl:value-of select="@disablefinancial" />
                </td>
                <td class="colW1">{lbl:disablefinancial_wc}</td>
                <td class="colW2">
                  <xsl:value-of select="@disablefinancial_wc" />
                </td>
                <td class="colW1">{lbl:disableoutright}</td>
                <td class="colW2">
                  <xsl:value-of select="@disableoutright" />
                </td>
                
              </tr>
              
            </xsl:when>
          </xsl:choose>
          

          <xsl:choose>
            <xsl:when test="$IsInternal = 0">


            </xsl:when>
            <xsl:otherwise>
              <tr>
                <td class="colW1">{lbl:creator}</td>
                <td class="colW2">

                  <xsl:value-of select="@creator" />
                </td>
                <td class="colW1">{lbl:recommend}</td>
                <td class="colW2">
                  <xsl:value-of select="@recommend" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
              </tr>
            </xsl:otherwise>
          </xsl:choose>


        </table>
      </xsl:when>
      <xsl:otherwise>
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
                      <td colspan="2"></td>
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
                      <td class="colTitle">{lbl:v2apt_9}</td>
                      <td class="colValue">
                        <xsl:value-of select="@v2apt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colTitle">{lbl:v2apt_20}</td>
                      <td class="colValue">
                        <xsl:value-of select="@v2apt_20" />
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
