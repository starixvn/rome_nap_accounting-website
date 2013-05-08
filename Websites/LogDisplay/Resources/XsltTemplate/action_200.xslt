<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>
    <xsl:template match="acc_customerinsmember_com">
        <table class="tblDesc">
            <!--<tr>
				-->
            <!--<td class="colW1">{lbl:custid}</td>
				<td class="colW2">
					<xsl:value-of select="@custid" />
				</td>-->
            <!--
				 <xsl:choose>
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
                </xsl:choose>
			
			</tr>-->
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
            <td class="colW1"></td>
            <td class="colW2"></td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>
    </xsl:template>

</xsl:stylesheet>
