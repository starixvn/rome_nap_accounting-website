<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerinsmember_com">
        <table class="tblDesc">
            <tr>
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
                </td></tr>
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
                </td> </tr>
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
                </td> </tr>
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
                </td>  </tr>
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
                </td>  </tr>
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
                </td> </tr>
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
                </td>  </tr>
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
              <td class="colW2">
                
              </td>
              <td class="colW1"></td>
              <td class="colW2"></td>
              
            </tr>
        </table>
    </xsl:template>

</xsl:stylesheet>
