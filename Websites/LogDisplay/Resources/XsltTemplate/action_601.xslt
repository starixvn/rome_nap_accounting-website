<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerinsmember_sag">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <!--<td class="colW1">{lbl:custid}</td>
                        <td class="colW2">
                            <xsl:value-of select="@custid" />
                        </td>-->
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
            <td class="colW1">{lbl:email}</td>
            <td class="colW2">
              <xsl:value-of select="@email" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:fax}</td>
            <td class="colW2">
              <xsl:value-of select="@fax" />
            </td>

            <td class="colW1">{lbl:address}</td>
            <td class="colW2">
              <xsl:value-of select="@address" />
            </td>
            <td class="colW1">{lbl:city}</td>
            <td class="colW2">
              <xsl:value-of select="@city" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:state}</td>
            <td class="colW2">
              <xsl:value-of select="@state" />
            </td>

            <td class="colW1">{lbl:country}</td>
            <td class="colW2">
              <xsl:value-of select="@country" />
            </td>
            <td class="colW1">{lbl:postal}</td>
            <td class="colW2">
              <xsl:value-of select="@postal" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:credit}</td>
            <td class="colW2">
              <xsl:value-of select="@credit" />
            </td>

            <td class="colW1">{lbl:TransferOption}</td>
            <td class="colW2" >
              <xsl:value-of select="@transferoption" />
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


            <td class="colW1">{lbl:IP}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
            <td class="colW1">{lbl:site}</td>
            <td class="colW2">
              <xsl:value-of select="@site" />
            </td>
          </tr>
          <xsl:choose>
            <xsl:when test="@minb">
              <tr>
                <td class="colW1">{lbl:groupa}</td>
                <td class="colW2">
                  <xsl:value-of select="@groupa" />
                </td>

                <td class="colW1">{lbl:groupb}</td>
                <td class="colW2">
                  <xsl:value-of select="@groupb" />
                </td>
                <td class="colW1">{lbl:groupc}</td>
                <td class="colW2">
                  <xsl:value-of select="@groupc" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:groupd}</td>
                <td class="colW2">
                  <xsl:value-of select="@groupd" />
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
                <td class="colW1">{lbl:discountnumber}</td>
                <td class="colW2">
                  <xsl:value-of select="@discountnumber" />
                </td>
                <td class="colW1">{lbl:minb}</td>
                <td class="colW2">
                  <xsl:value-of select="@minb" />
                </td>
                <td class="colW1">{lbl:maxb}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxb" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:mpm}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpm" />
                </td>
                <td class="colW1">{lbl:minbv2}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv2" />
                </td>
                <td class="colW1">{lbl:maxbv2}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv2" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:mpmv2}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv2" />
                </td>
                <td class="colW1">{lbl:minbv3}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv3" />
                </td>
                <td class="colW1">{lbl:maxbv3}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv3" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:mpmv3}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv3" />
                </td>
                <td class="colW1">{lbl:minbv5}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv5" />
                </td>
                <td class="colW1">{lbl:maxbv5}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv5" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:mpmv5}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv5" />
                </td>
                <td class="colW1">{lbl:minbv8}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv8" />
                </td>
                <td class="colW1">{lbl:maxbv8}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv8" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:mpmv8}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv8" />
                </td>
                <td class="colW1">{lbl:minbv10}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv10" />
                </td>
                <td class="colW1">{lbl:maxbv10}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv10" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:mpmv10}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv10" />
                </td>
                <td class="colW1">{lbl:minbv11}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv11" />
                </td>
                <td class="colW1">{lbl:maxbv11}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv11" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:mpmv11}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv11" />
                </td>
                <td class="colW1">{lbl:minbv99}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv99" />
                </td>
                <td class="colW1">{lbl:maxbv99}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv99" />
                </td>
              </tr>
              <tr>                
                <td class="colW1">{lbl:mpmv99}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv99" />
                </td>
                <td class="colW1">{lbl:MinBv99MP}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv99mp" />
                </td>
                <td class="colW1">{lbl:MaxBv99MP}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv99mp" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:MPMv99MP}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv99mp" />
                </td>
                <td class="colW1">{lbl:MinBv161}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbv161" />
                </td>
                <td class="colW1">{lbl:MaxBv161}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv161" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:MPMv161}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv161" />
                </td>
                <td class="colW1">{lbl:MPBv161}</td>
                <td class="colW2">
                  <xsl:value-of select="@mpbv161" />
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
                <td class="colW1">{lbl:disableautompt}</td>
                <td class="colW2">
                  <xsl:value-of select="@disableautompt" />
                </td>
                <td class="colW1"></td>
                <td class="colW2"></td>
              </tr>

            </xsl:when>
          </xsl:choose>
          <tr>
            <td class="colW1">{lbl:suspended}</td>
            <td class="colW2">
              <xsl:value-of select="@suspended" />
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
            <xsl:when test="$IsInternal = 0">

            </xsl:when>
            <xsl:otherwise>
              <tr>
                <td class="colW1">{lbl:srecommend}</td>
                <td class="colW2">

                  <xsl:value-of select="@srecommend" />
                </td>
                <td class="colW1">{lbl:creator}</td>
                <td class="colW2">

                  <xsl:value-of select="@creator" />
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
          <xsl:for-each select="s">
            <tr class="headerDetail" onclick="$('#s{{tbodylogid}}').slideToggle();" id="header{{tbodylogid}}" >
              <td colspan="6">{lbl:s}</td>
            </tr>
            <tr>
              <td colspan="6">
                <div  style="display:none;" id="s{{tbodylogid}}">
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
                  <table class="tblDesc" >
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
          <xsl:for-each select="m">
            <tr class="headerDetail" onclick="$('#m{{tbodylogid}}').slideToggle();">
              <td colspan="6">{lbl:m}</td>
            </tr>
            <tr>
              <td colspan="6">
                <div style="display:none;" id="m{{tbodylogid}}">
                  <table class="tblDesc" >
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
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
