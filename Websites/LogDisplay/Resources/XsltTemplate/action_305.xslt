<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmaster_minmaxbet">
        <table class="tblDesc">
            <tr>
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:custid}</td>
                        <td class="colW2">

                            <xsl:value-of select="@custid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->
                <!--<td class="colW1">{lbl:custid}</td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>-->

                <td class="colW1">{lbl:MinB}</td>
                <td class="colW2">
                    <xsl:value-of select="@minb" />
                </td>
                <td class="colW1">{lbl:MaxB}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxb" />
                </td>

                <td class="colW1">{lbl:MPM}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpm" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:MinBv2}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv2" />
                </td>
                <td class="colW1">{lbl:MaxBv2}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv2" />
                </td>

                <td class="colW1">{lbl:MPMv2}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv2" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:MinBv3}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv3" />
                </td>
                <td class="colW1">{lbl:MaxBv3}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv3" />
                </td>

                <td class="colW1">{lbl:MPMv3}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv3" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:MinBv5}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv5" />
                </td>
                <td class="colW1">{lbl:MaxBv5}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv5" />
                </td>

                <td class="colW1">{lbl:MPMv5}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv5" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:MinBv8}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv8" />
                </td>
                <td class="colW1">{lbl:MaxBv8}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv8" />
                </td>

                <td class="colW1">{lbl:MPMv8}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv8" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:MinBv10}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv10" />
                </td>
                <td class="colW1">{lbl:MaxBv10}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv10" />
                </td>

                <td class="colW1">{lbl:MPMv10}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv10" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:MinBv11}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv11" />
                </td>
                <td class="colW1">{lbl:MaxBv11}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv11" />
                </td>

                <td class="colW1">{lbl:MPMv11}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv11" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:MinBv99}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv99" />
                </td>
                <td class="colW1">{lbl:MaxBv99}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv99" />
                </td>

                <td class="colW1">{lbl:MPMv99}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv99" />
                </td>
            </tr>
            <tr>
              <td class="colW1">{lbl:MinBv99MP}</td>
              <td class="colW2">
                <xsl:value-of select="@minbv99mp" />
              </td>
              <td class="colW1">{lbl:MaxBv99MP}</td>
              <td class="colW2">
                <xsl:value-of select="@maxbv99mp" />
              </td>

              <td class="colW1">{lbl:MPMv99MP}</td>
              <td class="colW2">
                <xsl:value-of select="@mpmv99mp" />
              </td>
            </tr>
            <tr>
              <td class="colW1">{lbl:MinBv161}</td>
              <td class="colW2">
                <xsl:value-of select="@minbv161" />
              </td>
              <td class="colW1">{lbl:MaxBv161}</td>
              <td class="colW2">
                <xsl:value-of select="@maxbv161" />
              </td>

              <td class="colW1">{lbl:MPMv161}</td>
              <td class="colW2">
                <xsl:value-of select="@mpmv161" />
              </td>
            </tr>
            <tr>
              <td class="colW1">{lbl:MPBv161}</td>
              <td class="colW2">
                <xsl:value-of select="@mpbv161" />
              </td>
              <td class="colW1"></td>
              <td class="colW2"></td>
              <td class="colW1"></td>
              <td class="colW2"></td>
            </tr>
          
            <tr>
                <td class="colW1">{lbl:UpdMultiple}</td>
                <td class="colW2">
                    <xsl:value-of select="@updmultiple" />
                </td>
                <td class="colW1">{lbl:UMMB}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummb" />
                </td>

                <td class="colW1">{lbl:UMMBv2}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummbv2" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:UMMBv3}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummbv3" />
                </td>
                <td class="colW1">{lbl:UMMBv5}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummbv5" />
                </td>

                <td class="colW1">{lbl:UMMBv8}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummbv8" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:UMMBv10}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummbv10" />
                </td>
                <td class="colW1">{lbl:UMMBv11}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummbv11" />
                </td>

                <td class="colW1">{lbl:UMMBv99}</td>
                <td class="colW2">
                    <xsl:value-of select="@ummbv99" />
                </td>
            </tr>
            <tr>
              <td class="colW1">{lbl:UMMBv99MP}</td>
              <td class="colW2">
                <xsl:value-of select="@ummbv99" />
              </td>
              <td class="colW1">{lbl:UMMBv161}</td>
              <td class="colW2">
                <xsl:value-of select="@ummbv161" />
              </td>
              <td class="colW1"></td>
              <td class="colW2"></td>
            </tr>
          
            <tr>
                <td class="colW1">{lbl:SubAccName}</td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <!--<td class="colW1">{lbl:UserID}</td>
                <td class="colW2">
                    <xsl:value-of select="@userid" />
                </td>-->
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:userID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@userid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->

                <td class="colW1">{lbl:Language}</td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:ErrCode}</td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>

                <td class="colW1">{lbl:ErrMsg}</td>
                <td class="colW2" >
                    <xsl:value-of select="@errmsg" />
                </td>
                <td class="colW1"></td>
                <td class="colW2" >

                </td>


            </tr>
        </table>
    </xsl:template>

</xsl:stylesheet>
